namespace NewsPublish.Model.Response
{
    public class ResponseModel
    {
        public int Code { get; set; }
        public string Result { get; set; }
        public dynamic Data { get; set; }

        public ResponseModel()
            : this(0, string.Empty)
        {

        }

        public ResponseModel(int code, string result)
        {
            Code = code;
            Result = result;
        }
    }
}
