using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace brinux.ohnosolver.test
{
	public class OhNoCellSolverTest
	{
        private readonly ITestOutputHelper _output;

        public OhNoCellSolverTest(ITestOutputHelper output)
        {
            _output = output;

            Console.SetOut(new XunitConsoleOutput(_output));
        }

        [Fact]
		public void AvailabilitiesArelimitedToTheCellMaximumCapacity()
		{
			var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
			{
				OhNoCellSetup.SetupFullCell(1, 1, 1),
				OhNoCellSetup.SetupBlockedCell(2, 1)
			});

			var cellSolver = new OhNoCellSolver();

			var availabilities = cellSolver.CountAvailableCellsInDirection(
				OhNoDirectionEnum.RIGHT,
				new OhNoCellCoordinate(0, 0, schema),
				0);

			Assert.Equal(1, availabilities.CountMoves);
			Assert.Equal(1, availabilities.CountTotalLength);
		}

        [Fact]
        public void AvailabilitiesStopAtTheBorder()
        {
            var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(1, 1, 10),
                OhNoCellSetup.SetupBlockedCell(2, 1)
            });

            var cellSolver = new OhNoCellSolver();

            var availabilities = cellSolver.CountAvailableCellsInDirection(
                OhNoDirectionEnum.RIGHT,
                new OhNoCellCoordinate(0, 0, schema),
                0);

            Assert.Equal(3, availabilities.CountMoves);
            Assert.Equal(3, availabilities.CountTotalLength);
        }

        [Fact]
        public void AvailabilitiesStopAtBlocks()
        {
            var schema = new OhNoSchema(4, 4, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(1, 1, 10),
                OhNoCellSetup.SetupBlockedCell(2, 1),
                OhNoCellSetup.SetupBlockedCell(1, 4)
            });

            var cellSolver = new OhNoCellSolver();

            var availabilities = cellSolver.CountAvailableCellsInDirection(
                OhNoDirectionEnum.RIGHT,
                new OhNoCellCoordinate(0, 0, schema),
                0);

            Assert.Equal(2, availabilities.CountMoves);
            Assert.Equal(2, availabilities.CountTotalLength);
        }

        [Fact]
        public void AvailabilitiesConsiderFullCellIfConnected()
        {
            var schema = new OhNoSchema(1, 10, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(1, 1, 10),
                OhNoCellSetup.SetupFullCell(1, 3, 1),
                OhNoCellSetup.SetupBlockedCell(1, 4)
            });

            var cellSolver = new OhNoCellSolver();

            var availabilities = cellSolver.CountAvailableCellsInDirection(
                OhNoDirectionEnum.RIGHT,
                new OhNoCellCoordinate(0, 0, schema),
                0);

            Assert.Equal(1, availabilities.CountMoves);
            Assert.Equal(2, availabilities.CountTotalLength);
        }

        [Fact]
        public void AvailabilitiesConsidersBlocksAfterFullCells()
        {
            var schema = new OhNoSchema(1, 10, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(1, 1, 10),
                OhNoCellSetup.SetupFullCell(1, 3, 1),
                OhNoCellSetup.SetupBlockedCell(1, 5)
            });

            schema.Cells[0][3].Status = OhNoCellStatusEnum.Full;

            var cellSolver = new OhNoCellSolver();

            var availabilities = cellSolver.CountAvailableCellsInDirection(
                OhNoDirectionEnum.RIGHT,
                new OhNoCellCoordinate(0, 0, schema),
                0);

            Assert.Equal(1, availabilities.CountMoves);
            Assert.Equal(3, availabilities.CountTotalLength);
        }

        [Fact]
        public void AvailabilitiesConsidersMultipleFullSequences()
        {
            var schema = new OhNoSchema(1, 10, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(1, 1, 10),
                OhNoCellSetup.SetupFullCell(1, 3, 1),
                OhNoCellSetup.SetupFullCell(1, 6, 1),
                OhNoCellSetup.SetupBlockedCell(1, 8)
            });

            schema.Cells[0][3].Status = OhNoCellStatusEnum.Full;
            schema.Cells[0][6].Status = OhNoCellStatusEnum.Full;

            var cellSolver = new OhNoCellSolver();

            var availabilities = cellSolver.CountAvailableCellsInDirection(
                OhNoDirectionEnum.RIGHT,
                new OhNoCellCoordinate(0, 0, schema),
                0);

            Assert.Equal(2, availabilities.CountMoves);
            Assert.Equal(6, availabilities.CountTotalLength);
        }

        [Fact]
        public void AvailabilitiesStopsAtTheBorderAfterFullSequences()
        {
            var schema = new OhNoSchema(1, 10, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(1, 1, 10),
                OhNoCellSetup.SetupFullCell(1, 3, 1),
            });

            schema.Cells[0][3].Status = OhNoCellStatusEnum.Full;
            schema.Cells[0][4].Status = OhNoCellStatusEnum.Full;
            schema.Cells[0][5].Status = OhNoCellStatusEnum.Full;
            schema.Cells[0][6].Status = OhNoCellStatusEnum.Full;
            schema.Cells[0][7].Status = OhNoCellStatusEnum.Full;
            schema.Cells[0][8].Status = OhNoCellStatusEnum.Full;
            schema.Cells[0][9].Status = OhNoCellStatusEnum.Full;

            var cellSolver = new OhNoCellSolver();

            var availabilities = cellSolver.CountAvailableCellsInDirection(
                OhNoDirectionEnum.RIGHT,
                new OhNoCellCoordinate(0, 0, schema),
                0);

            Assert.Equal(1, availabilities.CountMoves);
            Assert.Equal(9, availabilities.CountTotalLength);
        }

        [Fact]
        public void IfTheCellIsAloneAxisLimitationsAreNotApplied()
        {
            var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(5, 5, 5)
            });

            var cellSolver = new OhNoCellSolver();

            var myCell = new OhNoCellCoordinate(4, 4, schema);

            var axisLimits = cellSolver.CountLimitsOnAxis(myCell, cellSolver.CountCellOwnSequenceLengths(myCell));

            Assert.Null(axisLimits[OhNoAxisEnum.UP_DOWN]);
            Assert.Null(axisLimits[OhNoAxisEnum.LEFT_RIGHT]);
        }

        [Fact]
        public void IfTheCellHasFullCellConnectedAxisLimitationAreNotApplied()
        {
            var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(5, 5, 5)
            });

            schema.Cells[4][3].Status = OhNoCellStatusEnum.Full;


            var cellSolver = new OhNoCellSolver();

            var myCell = new OhNoCellCoordinate(4, 4, schema);

            var axisLimits = cellSolver.CountLimitsOnAxis(myCell, cellSolver.CountCellOwnSequenceLengths(myCell));

            Assert.Null(axisLimits[OhNoAxisEnum.UP_DOWN]);
            Assert.Null(axisLimits[OhNoAxisEnum.LEFT_RIGHT]);
        }

        [Fact]
        public void IfTheCellHasFullCellConnectedOnBothAxisThenAxisLimitationAreNotApplied()
        {
            var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(5, 5, 5)
            });

            schema.Cells[4][3].Status = OhNoCellStatusEnum.Full;
            schema.Cells[3][4].Status = OhNoCellStatusEnum.Full;

            var cellSolver = new OhNoCellSolver();

            var myCell = new OhNoCellCoordinate(4, 4, schema);

            var axisLimits = cellSolver.CountLimitsOnAxis(myCell, cellSolver.CountCellOwnSequenceLengths(myCell));

            Assert.Null(axisLimits[OhNoAxisEnum.UP_DOWN]);
            Assert.Null(axisLimits[OhNoAxisEnum.LEFT_RIGHT]);
        }

        [Fact]
        public void IfTheCellHasConnectedValuedCellWithHigherValueLimitationAreNotApplied()
        {
            var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(5, 5, 5),
                OhNoCellSetup.SetupFullCell(5, 6, 6)
            });

            var cellSolver = new OhNoCellSolver();

            var myCell = new OhNoCellCoordinate(4, 4, schema);

            var axisLimits = cellSolver.CountLimitsOnAxis(myCell, cellSolver.CountCellOwnSequenceLengths(myCell));

            Assert.Null(axisLimits[OhNoAxisEnum.UP_DOWN]);
            Assert.Null(axisLimits[OhNoAxisEnum.LEFT_RIGHT]);
        }

        [Fact]
        public void IfTheCellHasConnectedValuedCellWithLowValueLimitationsAreApplied()
        {
            var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(5, 5, 6),
                OhNoCellSetup.SetupFullCell(5, 6, 2)
            });

            var cellSolver = new OhNoCellSolver();

            var myCell = new OhNoCellCoordinate(4, 4, schema);

            var axisLimits = cellSolver.CountLimitsOnAxis(myCell, cellSolver.CountCellOwnSequenceLengths(myCell));

            Assert.Equal(4, axisLimits[OhNoAxisEnum.UP_DOWN]);
            Assert.Equal(2, axisLimits[OhNoAxisEnum.LEFT_RIGHT]);
        }

        [Fact]
        public void IfTheCellHasConnectedValuedAndNotValuedCellsAxisLimitationIsApplied()
        {
            var schema = new OhNoSchema(9, 9, new List<OhNoCellSetup>()
            {
                OhNoCellSetup.SetupFullCell(5, 3, 8),
                OhNoCellSetup.SetupFullCell(5, 5, 7),
                OhNoCellSetup.SetupFullCell(5, 7, 5)
            });

            schema.Cells[4][3].Status = OhNoCellStatusEnum.Full;
            schema.Cells[4][5].Status = OhNoCellStatusEnum.Full;

            var cellSolver = new OhNoCellSolver();

            var myCell = new OhNoCellCoordinate(4, 4, schema);

            var axisLimits = cellSolver.CountLimitsOnAxis(myCell, cellSolver.CountCellOwnSequenceLengths(myCell));

            Assert.Equal(2, axisLimits[OhNoAxisEnum.UP_DOWN]);
            Assert.Equal(1, axisLimits[OhNoAxisEnum.LEFT_RIGHT]);
        }
    }
}