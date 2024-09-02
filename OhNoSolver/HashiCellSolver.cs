namespace brinux.hashisolver
{
	public class HashiCellSolver
	{
		public List<HashiCandidateConnection> SolveCell(HashiCellCoordinate cell)
		{
			var result = new List<HashiCandidateConnection>();

			Console.WriteLine($"Processing cell {cell.Row+1}:{cell.Column+1} - Value: {cell.Cell.Value}");

			var currentConnections = cell.CalculateCurrectConnections();
			var currentConnectionsTotal = currentConnections.Sum(d => d.Value);

			Console.Write("Existing connections: ");
			foreach (var connection in currentConnections)
			{
				Console.Write($"{connection.Value}{connection.Key.ToString().Substring(0, 1)} ");
			}
			Console.WriteLine($"--> {currentConnectionsTotal}");

			if (currentConnectionsTotal < cell.Cell.Value)
			{
				Console.WriteLine("Cell not yet solved!");

				var potentialConnections = CalculatePotentialConnections(cell, currentConnections);
				var potentialConnectionsTotal = potentialConnections.Sum(d => d.Value);

				Console.Write("Potential connections: ");
				foreach (var connection in potentialConnections)
				{
					Console.Write($"{connection.Value}{connection.Key.ToString().Substring(0, 1)} ");
				}
				Console.WriteLine($"--> {potentialConnectionsTotal}");

				var connectionOptions = CalculateOptions(currentConnectionsTotal, potentialConnections, cell.Cell.Value);

				if (connectionOptions.Count > 0)
				{
					Console.WriteLine($"Considering {connectionOptions.Count} connection options:");
					foreach (var connectionOption in connectionOptions)
					{
						foreach (var connection in connectionOption)
						{
							Console.Write($"{connection.ConnectionsNumber}{connection.Direciton.ToString().Substring(0, 1)} ");
						}
						Console.WriteLine();
					}
				}

				result = FindFixedConnections(connectionOptions);

				if (result.Count > 0)
				{
					Console.WriteLine("Found connections to apply:");
					foreach (var connection in result)
					{
						Console.Write($"{connection.ConnectionsNumber}{connection.Direciton.ToString().Substring(0, 1)} ");
					}
					Console.WriteLine();
				}
			}

			return result;
		}

		private Dictionary<DirectionEnum, int> CalculatePotentialConnections(HashiCellCoordinate cell, Dictionary<DirectionEnum, int> existingConnections)
		{
			var result = new Dictionary<DirectionEnum, int>();

			foreach (var direction in Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>())
			{
				var valuedCell = cell.MoveToNextValuedCell(direction);

				if (valuedCell.HasValue)
				{
					Console.WriteLine($"Found valued cell at {valuedCell.Value.Row+1}:{valuedCell.Value.Column+1}");

					if (valuedCell.HasValue && (
						!cell.Cell.ConnectionsGroup.HasValue ||
						!valuedCell.Value.Cell.ConnectionsGroup.HasValue ||
						cell.Cell.ConnectionsGroup.Value != valuedCell.Value.Cell.ConnectionsGroup.Value))
					{
						Console.WriteLine("Potential valid connection.");

						var valuedCellCurrentConnections = valuedCell.Value.CalculateCurrectConnections();

						Console.WriteLine($"The cell has {valuedCellCurrentConnections.Sum(d => d.Value)} connections in {valuedCellCurrentConnections.Count} directions");

						var existingConnectionsWithSourceCell = existingConnections.ContainsKey(direction) ? existingConnections[direction] : 0;

						if (valuedCell.Value.Cell.Value - valuedCellCurrentConnections.Sum(d => d.Value) > 0 && existingConnectionsWithSourceCell < 2)
						{
							result.Add(direction, 2 - existingConnectionsWithSourceCell);

							Console.WriteLine($"Considering potential connection towards {direction.ToString().Substring(0, 1)} of value {result[direction]}");
						}
					}
				}
			}

			return result;
		}

		private List<List<HashiCandidateConnection>> CalculateOptions(int current, Dictionary<DirectionEnum, int> potentials, int goal)
		{
			var result = new List<Dictionary<DirectionEnum, int>>();

			var potentialMoves = new List<HashiCandidateConnection>();

			foreach (var move in potentials)
			{
				potentialMoves.Add(new HashiCandidateConnection(move.Key, move.Value));
			}

			var movesPermutations = GetPermutations(potentialMoves);

			var options = new List<List<HashiCandidateConnection>>();

			foreach (var moveSet in movesPermutations)
			{
				var partial = current;
				var option = new List<HashiCandidateConnection>();

				foreach (var move in moveSet)
				{
					if (partial + move.ConnectionsNumber < goal)
					{
						partial += move.ConnectionsNumber;
						
						option.Add(move);
					}
					else
					{
						if (partial + move.ConnectionsNumber == goal)
						{
							option.Add(move);
						}
						else
						{
							var reducedMove = new HashiCandidateConnection(move.Direciton, goal - partial);

							option.Add(reducedMove);
						}

						options.Add(option);

						break;
					}
				}
			}

			return options;
		}

		private List<HashiCandidateConnection> FindFixedConnections(List<List<HashiCandidateConnection>> options)
		{
			var result = new List<HashiCandidateConnection>();

			foreach (var direction in Enum.GetValues(typeof(DirectionEnum)).Cast<DirectionEnum>())
			{
				if (options.All(o => o.Any(c => c.Direciton == direction)))
				{
					result.Add(new HashiCandidateConnection(direction, options.Min(o => o.Single(c => c.Direciton == direction).ConnectionsNumber)));
				}
			}

			return result;
		}

		static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list)
		{
			if (list.Count() == 1)
			{
				return new List<IEnumerable<T>> { list };
			}

			return list.SelectMany((element, index) =>
				GetPermutations(list.Where((e, i) => i != index))
				.Select(t => (new List<T> { element }).Concat(t)));
		}

		/*
		public bool SolveCell(HashiCellCoordinate cell)
		{
			var fullCellsCounts = CountCellOwnSequenceLengths(cell);
			var fullCellsTotal = fullCellsCounts.Sum(l => l.Value);

			var availabilities = CountAvailableCells(cell, fullCellsTotal);
			var availabilitiesTotal = availabilities.Sum(a => a.Value.CountTotalLength);

			var bidirectionalLimits = CountLimitsOnAxis(cell, fullCellsCounts);

			//Console.WriteLine($"Cell {cell.Row}:{cell.Column} - Count: {fullCellsCounts[OhNoDirectionEnum.TOP]}, {fullCellsCounts[OhNoDirectionEnum.RIGHT]}, {fullCellsCounts[OhNoDirectionEnum.BOTTOM]}, {fullCellsCounts[OhNoDirectionEnum.LEFT]} - Available: {availabilities[OhNoDirectionEnum.TOP].CountTotalLength}, {availabilities[OhNoDirectionEnum.RIGHT].CountTotalLength}, {availabilities[OhNoDirectionEnum.BOTTOM].CountTotalLength}, {availabilities[OhNoDirectionEnum.LEFT].CountTotalLength}");

			if (availabilitiesTotal < cell.Cell.Value - fullCellsTotal)
			{
				throw new Exception($"Error: it is not possible to satisfy cell {cell.Row}:{cell.Column} requirement of {cell.Cell.Value}.");
			}

			if (cell.Cell.Value < fullCellsTotal)
			{
				throw new Exception($"Error: cell {cell.Row}:{cell.Column} has {fullCellsTotal} connected cells, but should have only {cell.Cell.Value}");
			}

			if (cell.Cell.Value == fullCellsTotal)
			{
				AddFinalBlocks(cell);

				return true;
			}

			// Tutte le celle disponibili vanno riempite
			if (availabilitiesTotal == cell.Cell.Value - fullCellsTotal)
			{
				FillAllCellAvailabilities(cell, availabilities);

				return true;
			}

			// Prova ad aggiungere dei blocchi intorno alla cella
			if (AddMissingBlocks(cell, fullCellsTotal))
			{
				return true;
			}

			// Espandi in tutte le direzioni, se possibile
			if (ExpandCell(cell, fullCellsTotal, availabilities))
			{
				// Verifica se la cella è completa
				if (fullCellsTotal == cell.Cell.Value)
				{
					AddFinalBlocks(cell);
				}

				return true;
			}

			return false;
		}

		public Dictionary<AxisEnum, int?> CountLimitsOnAxis(HashiCellCoordinate startingCell, Dictionary<DirectionEnum, int> fullCellsCounts)
		{
			var limits = new Dictionary<AxisEnum, int?>();

			foreach (AxisEnum axis in Enum.GetValues(typeof(AxisEnum)))
			{
				Console.WriteLine($"Calculating limits on axis '{axis}' for cell {startingCell.Row + 1}:{startingCell.Column + 1}");

				limits.Add(axis, CountLimitOnAxis(startingCell, axis));
				
				Console.WriteLine("");
			}

			return limits;
		}

		private int? CountLimitOnAxis(HashiCellCoordinate startingCell, AxisEnum axis)
		{
			int? limit = null;

			// Definito l'asse, mi muovo in entrambe le sue direzioni
			foreach (var direction in axis.GetDirections())
			{
				var cell = startingCell;

				Console.WriteLine($"Working on axis '{axis}' in direction '{direction}' for cell {startingCell.Row + 1}:{startingCell.Column + 1}");

				// Per ogni direzione dell'asse...
				do
				{
					Console.WriteLine($"Analysis for cell {startingCell.Row + 1}:{startingCell.Column + 1} in direction '{direction}' is at cell {cell.Row + 1}:{cell.Column + 1}");

					// ...muovendomi nella direzione scelta, fintanto che la cella in cui mi trovo è piena (non dovrei saperlo a priori?)
					int? currentCellLimit = null;

					// ...se la cella ha valore...
					if (cell.Cell.IsValued)
					{
						Console.WriteLine($"Cell {cell.Row + 1}:{cell.Column + 1} has value: {cell.Cell.Value}");

						// calcolo il limite trasversale per quella cella, se c'è, muovendomi in entrambe le direzioni perpendicolari all'asse...
						// ...SALTANDO LA CELLA INIZIALE, ossia quella da cui sono partito. NB: mi sto muovendo sulla perpendicolare
						foreach (var secondaryDirection in direction.GetAxis().GetOppositeAxis().GetDirections())
						{
							if (cell.CanProceed(secondaryDirection))
							{
								var secondaryStartingCell = cell.Move(direction);

								if (!secondaryStartingCell.Cell.IsValued)
								{
									break;
								}

								do
								{
									// ...e proseguendo nella direzione scelta
									if (cell.CanProceed(secondaryDirection))
									{
										var nextCell = cell.Move(secondaryDirection);

										if (nextCell.Cell.IsValued)
										{
											if (nextCell.Cell.IsValued)
											{
												// Per tutte le cella con valore, analizzo la componente ulteriormente perpendicolare (qui probabilmente dovrei iterare...)
												foreach (var thirdDirection in axis.GetDirections())
												{
													int? nextCellLimit = null;
													//var nextCellLimit = CountLimitOnAxisDirection(nextCell, thirdDirection);

													Console.WriteLine($"Cell {nextCell.Row + 1}:{nextCell.Column + 1} is full with value {nextCell.Cell.Value}. Its limit in direction '{thirdDirection}' is: {nextCellLimit}");

													// Non c'è limite? -> il limite è la capacità della cella
													// Ha valore? -> il limite è la capacità della cella meno il limite

													nextCellLimit = nextCellLimit.HasValue ? nextCell.Cell.Value - nextCellLimit.Value : nextCell.Cell.Value;

													if (!currentCellLimit.HasValue || nextCellLimit.Value < currentCellLimit.Value)
													{
														currentCellLimit = nextCellLimit.Value;

														Console.WriteLine($"Limit for cell {cell.Row + 1}:{cell.Column + 1} on axis '{thirdDirection.GetAxis()}' updated to {nextCellLimit} while goind in direction '{thirdDirection}'");
													}
												}
											}
										}
										else
										{
											break;
										}
									}
									else
									{
										break;
									}
								} while (true);

							}


						}

						currentCellLimit = currentCellLimit.HasValue ? cell.Cell.Value - currentCellLimit.Value : cell.Cell.Value;

						if (!limit.HasValue || currentCellLimit.Value < limit.Value)
						{
							limit = currentCellLimit.Value;

							Console.WriteLine($"Overall limit for cell {startingCell.Row + 1}:{startingCell.Column + 1} updated to {limit}");

						}
					}

					if (cell.CanProceed(direction) && cell.Move(direction).Cell.IsValued)
					{
						cell = cell.Move(direction);

						Console.WriteLine($"Analysis for cell {startingCell.Row + 1}:{startingCell.Column + 1} can proceed in direction '{direction}' from cell {cell.Row + 1}:{cell.Column + 1}");
					}
					else
					{
						break;
					}

				} while (true);
			}

			Console.WriteLine($"Final limit for cell {startingCell.Row + 1}:{startingCell.Column + 1} is: {limit}");

			return limit;
		}

		private int? CountLimitOnAxisDirection(HashiCellCoordinate startingCell, DirectionEnum direction)
		{
			int? limit = null;

			var cell = startingCell;

			Console.WriteLine($"Starting limit analysis for cell {startingCell.Row + 1}:{startingCell.Column + 1} in direction '{direction}'");

			do
			{
				if (cell.Cell.IsValued)
				{
					Console.WriteLine($"Cell {cell.Row + 1}:{cell.Column + 1} is full");

					if (cell.Cell.IsValued)
					{
						Console.WriteLine($"Cell {cell.Row + 1}:{cell.Column + 1} has value: {cell.Cell.Value}. Analyzing opposite axis...");

						// Check availability on the other axis
						var cellLimit = CountLimitOnAxis(cell, direction.GetAxis().GetOppositeAxis());

						Console.WriteLine($"limit analysis for cell {cell.Row + 1}:{cell.Column + 1} on axis '{direction.GetAxis().GetOppositeAxis()}' has value: {cellLimit}");

						// Count limit on current axis
						var currentCellLimit = cell.Cell.Value - (cellLimit.HasValue ? cellLimit.Value : 0);
						if (cellLimit.HasValue && (!limit.HasValue || cell.Cell.Value - currentCellLimit < limit))
						{
							limit = cell.Cell.Value - currentCellLimit;

							Console.WriteLine($"Limit for cell {startingCell.Row + 1}:{startingCell.Column + 1} in direction '{direction}' updated to: {limit}");
						}
					}

					if (cell.CanProceed(direction))
					{
						cell = cell.Move(direction);

						Console.WriteLine($"Moving in direction '{direction}' to cell {cell.Row + 1}:{cell.Column + 1}");
					}
					else
					{
						Console.WriteLine($"Cannot proceed from cell {cell.Row + 1}:{cell.Column + 1} in direction '{direction}'");

						break;
					}
				}
				else
				{
					Console.WriteLine($"Cell {cell.Row + 1}:{cell.Column + 1} is not; ending directional analysis");

					break;
				}
			} while (true);

			Console.WriteLine($"Limit for cell {startingCell.Row + 1}:{startingCell.Column + 1} in direction '{direction}' is: {limit}");

			return limit;
		}

		public int CountSequenceLimitOnAxis(HashiCellCoordinate startingCell, AxisEnum axis)
		{
			var axisLimit = startingCell.Cell.Value;

			foreach (var direction in axis.GetDirections())
			{
				var cell = startingCell;

				while (cell.CanProceed(direction))
				{
					var nextCell = cell.Move(direction);

					if (nextCell.Cell.IsValued)
					{
						if (nextCell.Cell.IsValued)
						{
							var nextCellLimit = nextCell.Cell.Value - CountCellOwnSequenceLengths(nextCell).Where(d => axis.GetOppositeAxis().GetDirections().Contains(d.Key)).Sum(d => d.Value);

							if (nextCellLimit < axisLimit)
							{
								axisLimit = nextCellLimit;
							}
						}

						cell = nextCell;
					}
					else
					{
						break;
					}
				}
			}

			return axisLimit;
		}

		public void FillAllCellAvailabilities(HashiCellCoordinate cell, Dictionary<DirectionEnum, OhNoCellMoves> availabilities)
		{
			foreach (var availabilitiesInDirection in availabilities.Values.ToList())
			{
				if (availabilitiesInDirection.CountMoves > 0)
				{
					foreach (var availability in availabilitiesInDirection)
					{
						availability.Coordinates.Cell.Status = HashiCellStatusEnum.Valued;

						Console.WriteLine($"Cell {availability.Coordinates.Row + 1}:{availability.Coordinates.Column + 1} was filled since all availabilities for cell {cell.Row + 1}:{cell.Column + 1} must be to satify its requirement of {cell.Cell.Value} in direction '{availabilitiesInDirection.Direction}'");
					}

					var lastMove = availabilitiesInDirection.Last();

					if (lastMove.Coordinates.CanProceed(availabilitiesInDirection.Direction, lastMove.Length))
					{
						var finalCell = lastMove.Coordinates.Move(availabilitiesInDirection.Direction, lastMove.Length);

						if (finalCell.Cell.IsEmpty)
						{
							finalCell.Cell.Status = HashiCellStatusEnum.Connection;

							Console.WriteLine($"Cell {finalCell.Row + 1}:{finalCell.Column + 1} was blocked in direction '{availabilitiesInDirection.Direction}' after adding the required cells for cell {cell.Row + 1}:{cell.Column + 1}");
						}
					}
				}
			}

			AddFinalBlocks(cell);
		}

		public bool ExpandCell(HashiCellCoordinate currentCell, int fullCellsTotal, Dictionary<DirectionEnum, OhNoCellMoves> availabilities)
		{
			var result = false;

			foreach (DirectionEnum direction in Enum.GetValues(typeof(DirectionEnum)))
			{
				result |= ExpandCellInDirection(currentCell, fullCellsTotal, direction, availabilities);
			}

			return result;
		}

		public bool ExpandCellInDirection(HashiCellCoordinate currentCell, int fullCellsTotal, DirectionEnum direction, Dictionary<DirectionEnum, OhNoCellMoves> availabilities)
		{
			var result = false;

			var availabilitiesCountInTheOtherDirections = availabilities.Where(a => a.Key != direction).Sum(a => a.Value.CountTotalLength);

			if (availabilitiesCountInTheOtherDirections + fullCellsTotal < currentCell.Cell.Value && availabilities[direction].CountMoves > 0)
			{
				//Console.WriteLine($"Should expand on top - Current: {fullCellsTotal} - Target: {currentCell.Cell.Value} - Available: {availabilities[direction].CountTotalLength} in {availabilities[direction].CountMoves} moves");

				var missingCoverage = currentCell.Cell.Value - fullCellsTotal - availabilitiesCountInTheOtherDirections;
				var addedCoverage = 0;

				foreach (var availability in availabilities[direction])
				{
					//if (missingCoverage >= addedCoverage + 1 + availability.Length)
					if (missingCoverage >= addedCoverage + 1)
					{
						availability.Coordinates.Cell.Status = HashiCellStatusEnum.Valued;
						addedCoverage += availability.Length;

						Console.WriteLine($"Filled cell {availability.Coordinates.Row + 1}:{availability.Coordinates.Column + 1} from cell {currentCell.Row + 1}:{currentCell.Column + 1} in direction '{direction}' - Total length: +{availability.Length}");

						result = true;
					}
					else
					{
						break;
					}
				}
			}

			return result;
		}

		public bool AddFinalBlocks(HashiCellCoordinate cell)
		{
			var result = false;

			foreach (DirectionEnum direction in Enum.GetValues(typeof(DirectionEnum)))
			{
				result |= AddFinalBlockInDirection(direction, cell);
			}

			if (!cell.Cell.IsSolved)
			{
				cell.Cell.IsSolved = true;

				Console.WriteLine($"Cell {cell.Row + 1}:{cell.Column + 1} is completed");

				result = true;
			}

			return result;
		}

		public bool AddFinalBlockInDirection(DirectionEnum direction, HashiCellCoordinate startingCell)
		{
			var result = false;

			var cell = startingCell;

			while (cell.CanProceed(direction))
			{
				var nextCell = cell.Move(direction);

				if (!nextCell.Cell.IsValued)
				{
					break;
				}

				cell = nextCell;
			}

			if (cell.CanProceed(direction))
			{
				var nextCell = cell.Move(direction);

				if (nextCell.Cell.IsEmpty)
				{
					nextCell.Cell.Status = HashiCellStatusEnum.Connection;

					Console.WriteLine($"Blocked cell {nextCell.Row + 1}:{nextCell.Column + 1} from cell {startingCell.Row + 1}:{startingCell.Column + 1} completion in direction '{direction}'");

					result = true;
				}
			}

			return result;
		}

		public bool AddMissingBlocks(HashiCellCoordinate cell, int totalCount)
		{
			var result = false;

			foreach (DirectionEnum direction in Enum.GetValues(typeof(DirectionEnum)))
			{
				result |= AddMissingBlockInDirection(direction, cell, totalCount);
			}

			return result;
		}

		public bool AddMissingBlockInDirection(DirectionEnum direction, HashiCellCoordinate startingCell, int totalCount)
		{
			var result = false;

			var cell = startingCell;

			while (cell.CanProceed(direction))
			{
				var nextCell = cell.Move(direction);

				if (!nextCell.Cell.IsValued)
				{
					break;
				}

				cell = nextCell;
			}

			if (cell.CanProceed(direction) && cell.CanProceed(direction, 2))
			{
				var nextCell = cell.Move(direction);
				var followingCell = cell.Move(direction, 2);

				if (nextCell.Cell.IsEmpty && followingCell.Cell.IsValued)
				{
					var sequenceLength = CountSequenceLengthInDirection(direction, followingCell);

					if (startingCell.Cell.Value < totalCount + sequenceLength + 1)
					{
						nextCell.Cell.Status = HashiCellStatusEnum.Connection;

						Console.WriteLine($"Blocked cell {nextCell.Row + 1}:{nextCell.Column + 1} from cell {startingCell.Row + 1}:{startingCell.Column + 1} in direction '{direction}'.");

						result = true;
					}
				}
			}

			return result;
		}

		public Dictionary<DirectionEnum, OhNoCellMoves> CountAvailableCells(HashiCellCoordinate startingCell, int currentCellsCount)
		{
			var counts = new Dictionary<DirectionEnum, OhNoCellMoves>();

			foreach (DirectionEnum direction in Enum.GetValues(typeof(DirectionEnum)))
			{
				counts.Add(direction, CountAvailableCellsInDirection(direction, startingCell, currentCellsCount));
			}

			return counts;
		}

		public OhNoCellMoves CountAvailableCellsInDirection(DirectionEnum direction, HashiCellCoordinate startingCell, int currentCellsCount)
		{
			var cell = startingCell;
			var availabilities = new OhNoCellMoves(direction);

			while (cell.CanProceed(direction))
			{
				cell = cell.Move(direction);

				if (!cell.Cell.IsValued)
				{
					break;
				}
			}

			if (cell.Cell.IsEmpty)
			{
				do
				{
					// Is there a cell ahed?
					if (!cell.CanProceed(direction))
					{
						// No: add move, and exit
						availabilities.Add(new OhNoCellMove(cell, 1));

						return availabilities;
					}
					else
					{
						var nextCell = cell.Move(direction);

						// Yes: of what kind?
						switch (nextCell.Cell.Status)
						{
							// Empty: add move, and go on
							case HashiCellStatusEnum.Empty:
								availabilities.Add(new OhNoCellMove(cell, 1));

								if (currentCellsCount + availabilities.CountTotalLength == startingCell.Cell.Value)
								{
									return availabilities;
								}

								cell = nextCell;
								break;

							// Block: add move, and exit
							case HashiCellStatusEnum.Connection:
								availabilities.Add(new OhNoCellMove(cell, 1));

								return availabilities;

							// Full: how long is the sequence?
							case HashiCellStatusEnum.Valued:
								int length = CountSequenceLengthInDirection(direction, nextCell);

								bool hasEquenceLimits = false;
								int? sequenceLimit = null;

								// As much as the target: add move (empty cell + sequence length), and exit
								if (length + currentCellsCount + availabilities.CountTotalLength == startingCell.Cell.Value)
								{
									availabilities.Add(new OhNoCellMove(cell, length + 1));

									return availabilities;
								}
								// Less than the target: add move (empty cell + sequence length), and go on							
								else if (length + currentCellsCount + availabilities.CountTotalLength < startingCell.Cell.Value)
								{
									availabilities.Add(new OhNoCellMove(cell, length + 1));

									if (nextCell.CanProceed(direction, length))
									{
										cell = nextCell.Move(direction, length);

										if (cell.Cell.IsConnection)
										{
											return availabilities;
										}
									}
									else
									{
										return availabilities;
									}
								}
								// More than the target: exit
								else
								{
									return availabilities;
								}

								break;
						}
					}
				} while (true);
			}

			return availabilities;
		}

		public Dictionary<DirectionEnum, int> CountCellOwnSequenceLengths(HashiCellCoordinate cell)
		{
			return CountSequenceLengths(cell).ToDictionary(i => i.Key, i => i.Value - 1);
		}

		public Dictionary<DirectionEnum, int> CountSequenceLengths(HashiCellCoordinate cell)
		{
			var counts = new Dictionary<DirectionEnum, int>();

			foreach (DirectionEnum direction in Enum.GetValues(typeof(DirectionEnum)))
			{
				counts.Add(direction, CountSequenceLengthInDirection(direction, cell));
			}

			return counts;
		}

		public int CountSequenceLengthInDirection(DirectionEnum direction, HashiCellCoordinate cell)
		{
			var currentCell = cell;
			int count = 0;

			while (currentCell.Cell.Status == HashiCellStatusEnum.Valued)
			{
				count++;

				if (currentCell.CanProceed(direction))
				{
					currentCell = currentCell.Move(direction);
				}
				else
				{
					break;
				}
			}

			return count;
		}

		public int CountSequenceAvailableLengthInDirection(DirectionEnum direction, HashiCellCoordinate cell)
		{
			var currentCell = cell;
			int count = 0;

			while (currentCell.Cell.Status == HashiCellStatusEnum.Valued)
			{
				count++;

				if (currentCell.CanProceed(direction))
				{
					currentCell = currentCell.Move(direction);
				}
				else
				{
					break;
				}
			}

			return count;
		}*/
	}
}