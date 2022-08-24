namespace Heus.Ddd.Application;

    public class PageRequestDto
    {
        /// <summary>
        /// Default value: 10.
        /// </summary>
        public static int DefaultMaxResultCount { get; set; } = 10;

        /// <summary>
        /// Maximum possible value of the <see cref="MaxResultCount"/>.
        /// Default value: 1,000.
        /// </summary>
        public static int MaxResultCount { get; set; } = 1000;
    }
