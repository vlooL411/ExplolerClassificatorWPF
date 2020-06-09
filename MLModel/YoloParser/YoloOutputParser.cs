using ExplolerClassificatorWPF.ML.YoloParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;

namespace ExplolerClassificatorWPF.MLModel.Yolo.YoloParser
{
    class YoloOutputParser
    {
        public const int ROW_COUNT = 13;
        public const int COL_COUNT = 13;
        public const int CHANNEL_COUNT = 125;
        public const int BOXES_PER_CELL = 5;
        public const int BOX_INFO_FEATURE_COUNT = 5;
        public const int CLASS_COUNT = 20;
        public const float CELL_WIDTH = 32;
        public const float CELL_HEIGHT = 32;

        const int channelStride = ROW_COUNT * COL_COUNT;

        float[] anchors = new float[] { 1.08F, 1.19F, 3.42F, 4.41F, 6.63F, 11.38F, 9.42F, 5.11F, 16.62F, 10.52F };

        static public string[] labels = new string[]
        {
            "aeroplane", "bicycle", "bird", "boat", "bottle",
            "bus", "car", "cat", "chair", "cow",
            "diningtable", "dog", "horse", "motorbike", "person",
            "pottedplant", "sheep", "sofa", "train", "tvmonitor",
        };
        static public Color[] Colors = new Color[]
       {
            System.Windows.Media.Colors.Khaki,
            System.Windows.Media.Colors.Fuchsia,
            System.Windows.Media.Colors.Silver,
            System.Windows.Media.Colors.RoyalBlue,
            System.Windows.Media.Colors.Green,
            System.Windows.Media.Colors.DarkOrange,
            System.Windows.Media.Colors.Purple,
            System.Windows.Media.Colors.Gold,
            System.Windows.Media.Colors.Red,
            System.Windows.Media.Colors.Aquamarine,
            System.Windows.Media.Colors.Lime,
            System.Windows.Media.Colors.AliceBlue,
            System.Windows.Media.Colors.Sienna,
            System.Windows.Media.Colors.Orchid,
            System.Windows.Media.Colors.Tan,
            System.Windows.Media.Colors.LightPink,
            System.Windows.Media.Colors.Yellow,
            System.Windows.Media.Colors.HotPink,
            System.Windows.Media.Colors.OliveDrab,
            System.Windows.Media.Colors.SandyBrown,
            System.Windows.Media.Colors.DarkTurquoise
       };

        float Sigmoid(float value)
        {
            var k = MathF.Exp(value);
            return k / (1.0f + k);
        }

        float[] Softmax(float[] values)
        {
            var exp = values.Select(v => MathF.Exp(v - values.Max()));
            var sumExp = exp.Sum();

            return exp.Select(v => v / sumExp).ToArray();
        }

        int GetOffset(int x, int y, int channel) => channel * channelStride + y * COL_COUNT + x;

        DimensionsBase ExtractBoundingBoxDimensions(float[] modelOutput, int x, int y, int channel)
            => new DimensionsBase
            {
                X = modelOutput[GetOffset(x, y, channel)],
                Y = modelOutput[GetOffset(x, y, channel + 1)],
                Width = modelOutput[GetOffset(x, y, channel + 2)],
                Height = modelOutput[GetOffset(x, y, channel + 3)]
            };

        float GetConfidence(float[] modelOutput, int x, int y, int channel)
           => Sigmoid(modelOutput[GetOffset(x, y, channel + 4)]);

        DimensionsBase MapBoundingBoxToCell(int x, int y, int box, DimensionsBase boxDimensions)
            => new DimensionsBase
            {
                X = (x + Sigmoid(boxDimensions.X)) * CELL_WIDTH,
                Y = (y + Sigmoid(boxDimensions.Y)) * CELL_HEIGHT,
                Width = MathF.Exp(boxDimensions.Width) * CELL_WIDTH * anchors[box * 2],
                Height = MathF.Exp(boxDimensions.Height) * CELL_HEIGHT * anchors[box * 2 + 1],
            };

        public float[] ExtractClasses(float[] modelOutput, int x, int y, int channel)
        {
            var predictedClasses = new float[CLASS_COUNT];
            int predictedClassOffset = channel + BOX_INFO_FEATURE_COUNT;
            for (int predictedClass = 0; predictedClass < CLASS_COUNT; predictedClass++)
                predictedClasses[predictedClass] = modelOutput[GetOffset(x, y, predictedClass + predictedClassOffset)];
            return Softmax(predictedClasses);
        }

        (int, float) GetTopResult(float[] predictedClasses)
            => predictedClasses.Select((predictedClass, index) => (Index: index, Value: predictedClass))
                               .OrderByDescending(result => result.Value).First();

        public IEnumerable<YoloBoundingBox> ParseOutputs(float[] yoloModelOutputs, float threshold = .3F)
        {
            for (int row = 0; row < ROW_COUNT; row++)
                for (int column = 0; column < COL_COUNT; column++)
                    for (int box = 0; box < BOXES_PER_CELL; box++)
                    {
                        var channel = box * (CLASS_COUNT + BOX_INFO_FEATURE_COUNT);

                        var boundingBoxDimensions = ExtractBoundingBoxDimensions(yoloModelOutputs, row, column, channel);

                        float confidence = GetConfidence(yoloModelOutputs, row, column, channel);

                        if (confidence < threshold) continue;

                        var mappedBoundingBox = MapBoundingBoxToCell(row, column, box, boundingBoxDimensions);

                        float[] predictedClasses = ExtractClasses(yoloModelOutputs, row, column, channel);

                        var (topResultIndex, topResultScore) = GetTopResult(predictedClasses);
                        var topScore = topResultScore * confidence;

                        if (topScore < threshold) continue;

                        yield return new YoloBoundingBox()
                        {
                            Dimensions = new DimensionsBase
                            {
                                X = mappedBoundingBox.X - mappedBoundingBox.Width / 2,
                                Y = mappedBoundingBox.Y - mappedBoundingBox.Height / 2,
                                Width = mappedBoundingBox.Width,
                                Height = mappedBoundingBox.Height,
                            },
                            Confidence = topScore,
                            Label = labels[topResultIndex],
                            BoxColor = Colors[topResultIndex % Colors.Length]
                        };
                    }
        }

        float IntersectionOverUnion(DimensionsBase BoxA, DimensionsBase BoxB)
        {
            var areaA = BoxA.Width * BoxA.Height;
            if (areaA <= 0) return 0;
            var areaB = BoxB.Width * BoxB.Height;
            if (areaB <= 0) return 0;

            var minX = Math.Max(BoxA.X, BoxB.X);
            var minY = Math.Max(BoxA.Y, BoxB.Y);
            var maxX = Math.Min(BoxA.X + BoxA.Width, BoxB.X + BoxB.Width);
            var maxY = Math.Min(BoxA.Y + BoxA.Height, BoxB.Y + BoxB.Height);

            var intersectionArea = Math.Max(maxY - minY, 0) * Math.Max(maxX - minX, 0);

            return intersectionArea / (areaA + areaB - intersectionArea);
        }
        public IEnumerable<YoloBoundingBox> FilterBoundingBoxes(IEnumerable<YoloBoundingBox> boxes, int limit, float threshold)
        {
            var boxesList = boxes.ToList();
            var activeCount = boxesList.Count;
            var isActiveBoxes = new bool[boxesList.Count];

            for (int i = 0; i < isActiveBoxes.Length; i++)
                isActiveBoxes[i] = true;

            var sortedBoxes = boxes.Select((b, i) => new { Box = b, Index = i })
                                .OrderByDescending(b => b.Box.Confidence).ToList();

            var results = new List<YoloBoundingBox>();

            for (int i = 0; i < boxesList.Count; i++)
            {
                if (!isActiveBoxes[i]) continue;

                var boxA = sortedBoxes[i].Box;
                results.Add(boxA);

                if (results.Count >= limit) break;

                for (var j = i + 1; j < boxesList.Count; j++)
                    if (isActiveBoxes[j])
                    {
                        var boxB = sortedBoxes[j].Box;

                        if (IntersectionOverUnion(boxA.Dimensions, boxB.Dimensions) > threshold)
                        {
                            isActiveBoxes[j] = false;
                            if (--activeCount <= 0) break;
                        }
                    }

                if (activeCount <= 0) break;
            }
            return results;
        }
    }
}