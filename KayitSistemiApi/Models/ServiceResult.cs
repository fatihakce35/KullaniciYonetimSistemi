namespace KayitSistemiApi.Models
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { set; get; } = false;
        public T Data { set; get; }
        public string Message { set; get; }

        public int StatusCode { get; set; } = 0;

      


    }
}
