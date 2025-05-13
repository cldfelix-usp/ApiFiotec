namespace ApiFiotec.Application.Dtos.Response
{
    public class BaseResponseViewModel<T>
    {
        public T? Data { get; private set; }
        public List<string> Errors { get; } = [];

        public BaseResponseViewModel(T data, List<string> errors)
        {
            Data = data;
            Errors = errors;
        }

        public BaseResponseViewModel(List<string> errors) => Errors = errors;

        public BaseResponseViewModel(T data) => Data = data;

        public BaseResponseViewModel(string error) => Errors.Add(error);

    }
}
