namespace HxLabsAdvanced.APIService.Helpers
{
    //REF 10 Paginación
    public class MoviesResourceParameters
    {
        private const int maxPageSize = 20;

        private int pageSize = 10;

        public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
