namespace Server.Utils {
    public struct ResultInfo<T> {
        public Status status;
        public T content;
        public string message;
        
        public ResultInfo(Status status, T content, string message = "") {
            this.status = status;
            this.content = content;
            this.message = message;
        }
    }
}