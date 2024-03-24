namespace ApiNetCore.Application.Procedures.Files
{
    internal class FileSizeChecker
    {
        private static FileSizeChecker? _instance;
        public static FileSizeChecker Instance
        {
            get
            {
                if (_instance is null)
                    _instance = new FileSizeChecker();
                return _instance;
            }
        }

        public double GetKilobytesSize(ulong bytesLength)
        {
            return bytesLength * 1024;
        }

        public double GetMegabytesSize(ulong bytesLength)
        {
            return GetKilobytesSize(bytesLength) * 1024;
        }
    }
}
