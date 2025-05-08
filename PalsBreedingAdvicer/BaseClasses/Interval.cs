namespace PalsBreedingAdvicer
{
    internal class Interval
    {
        public float LeftEndpoint { get; private set; }
        public float RightEndpoint { get; private set; }
        public IntervalEndpoints Endpoints { get; private set; }

        private bool isLeftClosed, isRightClosed;




        public Interval(float leftBorder, float rightBorder, IntervalEndpoints segmentBorders = IntervalEndpoints.Closed)
        {
            if (leftBorder < rightBorder) {
                LeftEndpoint = leftBorder;
                RightEndpoint = rightBorder;
            } else {
                LeftEndpoint = rightBorder;
                RightEndpoint = leftBorder;
            }

            isLeftClosed = Endpoints == IntervalEndpoints.Closed || Endpoints == IntervalEndpoints.RightOpen;
            isRightClosed = Endpoints == IntervalEndpoints.Closed || Endpoints == IntervalEndpoints.LeftOpen;
        }


        public Interval(float leftBorder, float rightBorder, bool isLeftClosed = true, bool isRightClosed = true)
        {
            if (leftBorder < rightBorder) {
                LeftEndpoint = leftBorder;
                RightEndpoint = rightBorder;
            } else {
                LeftEndpoint = rightBorder;
                RightEndpoint = leftBorder;
            }
            this.isLeftClosed = isLeftClosed;
            this.isRightClosed = isRightClosed;
            byte endpoints = 0;
            if (!isLeftClosed) endpoints += 1;
            if (!isRightClosed) endpoints += 2;
            Endpoints = (IntervalEndpoints)endpoints;
        }


        public bool Contains(float value)
        {
            bool toTheRight = value > LeftEndpoint && !isLeftClosed || value >= LeftEndpoint && isLeftClosed;
            bool toTheLeft = value < RightEndpoint && !isRightClosed || value <= RightEndpoint && isRightClosed;
            return toTheRight && toTheLeft;
        }

        public bool Above(float value)
        {
            return (value > RightEndpoint && isRightClosed || value >= RightEndpoint && !isRightClosed);
        }

        public bool Below(float value)
        {
            return (value < LeftEndpoint && isLeftClosed || value <= LeftEndpoint && !isLeftClosed);
        }

        public override string ToString()
        {
            var leftBracket = isLeftClosed ? '[' : '(';
            var rightBracket = isRightClosed ? ']' : ')';
            return $"{leftBracket}{LeftEndpoint}, {RightEndpoint}{rightBracket}";
        }
    }
}
