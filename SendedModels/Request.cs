using System;

namespace SendedModels
{
    public delegate void OnRequestFinished(object sender, object reciever);

    [Serializable]
    public class Request
    {
        public Request()
        {
        }

        public Request(string requestID, DateTime sendedDate, DateTime arrivedDate, RequestState state, RequestType requestCommand, byte[] data)
        {
            SetDatas(requestID, sendedDate, arrivedDate, state, requestCommand, data);
        }

        public Request(string requestID, DateTime arrivedDate, RequestState state, RequestType requestCommand, byte[] data)
        {
            SetDatas(requestID, DateTime.Now, arrivedDate, state, requestCommand, data);
        }

        public Request(string requestID, DateTime sendedDate, DateTime arrivedDate, RequestType requestCommand, byte[] data)
        {
            SetDatas(requestID, sendedDate, arrivedDate, RequestState.Created, requestCommand, data);
        }

        public Request(string requestID, RequestType requestCommand, DateTime arrivedDate, byte[] data)
        {
            SetDatas(requestID, DateTime.Now, arrivedDate, RequestState.Created, requestCommand, data);
        }

        public string GenerateID()
        {
            var num = (long)(new Random().Next(int.MaxValue / 2, int.MaxValue) * int.MaxValue);
            var time = DateTime.Now;

            return $"{time.Year}{time.Month}{time.Day}{time.Hour}{time.Minute}{time.Second}{time.Millisecond}{num}";
        }

        private void SetDatas(string requestID, DateTime sendedDate, DateTime arrivedDate, RequestState state, RequestType requestCommand, byte[] data)
        {
            RequestID = requestID ?? throw new ArgumentNullException(nameof(requestID));
            CreatedDate = sendedDate;
            ArrivedDate = arrivedDate;
            State = state;
            Command = requestCommand;
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        public string RequestID { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ArrivedDate { get; set; }
        public RequestState State { get; set; }
        public DateTime FinishedDate { get; set; }
        public RequestType Command { get; set; }
        public byte[] Data { get; set; }
    }
}