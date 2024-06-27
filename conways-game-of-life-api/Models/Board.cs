using System;
using System.Collections.Generic;

namespace conways_game_of_life_api.Models
{
    /// <summary>
    /// Represents a board in Conway's Game of Life.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// Gets or sets the unique identifier for the board.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the width of the grid.
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the grid.
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the list of live cells' positions.
        /// </summary>
        public List<(int X, int Y)> LiveCells { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        public Board()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class with specified dimensions.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        public Board(int width, int height)
        {
            Width = width;
            Height = height;
            LiveCells = new List<(int X, int Y)>();
        }
    }
}
