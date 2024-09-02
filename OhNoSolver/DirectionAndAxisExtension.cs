namespace brinux.hashisolver
{
    static class DirectionAndAxisExtension
    {
        public static DirectionEnum GetOppositeDirection(this DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    return DirectionEnum.DOWN;
                case DirectionEnum.RIGHT:
                    return DirectionEnum.LEFT;
                case DirectionEnum.DOWN:
                    return DirectionEnum.UP;
                case DirectionEnum.LEFT:
                    return DirectionEnum.RIGHT;
                default:
                    throw new NotImplementedException();
            }
        }

        public static AxisEnum GetAxis(this DirectionEnum direction)
        {
            switch (direction)
            {
                case DirectionEnum.UP:
                    return AxisEnum.UP_DOWN;
                case DirectionEnum.RIGHT:
                    return AxisEnum.LEFT_RIGHT;
                case DirectionEnum.DOWN:
                    return AxisEnum.UP_DOWN;
                case DirectionEnum.LEFT:
                    return AxisEnum.LEFT_RIGHT;
                default:
                    throw new NotImplementedException();
            }
        }

        public static List<DirectionEnum> GetDirections(this AxisEnum direction)
        {
            switch (direction)
            {
                case AxisEnum.UP_DOWN:
                    return new List<DirectionEnum> { DirectionEnum.UP, DirectionEnum.DOWN };
                case AxisEnum.LEFT_RIGHT:
                    return new List<DirectionEnum> { DirectionEnum.LEFT, DirectionEnum.RIGHT };
                default:
                    throw new NotImplementedException();
            }
        }

        public static AxisEnum GetOppositeAxis(this AxisEnum direction)
        {
            switch (direction)
            {
                case AxisEnum.UP_DOWN:
                    return AxisEnum.LEFT_RIGHT;
                case AxisEnum.LEFT_RIGHT:
                    return AxisEnum.UP_DOWN;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}