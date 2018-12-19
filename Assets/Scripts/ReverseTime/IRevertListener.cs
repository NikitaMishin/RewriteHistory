namespace ReverseTime
{
    public interface IRevertListener
    {
        /// <summary>
        /// Record current position with transformation
        /// </summary>
        void RecordTimePoint();

        /// <summary>
        /// Start rewind time and remove  last point
        /// </summary>
        void StartRewind();


        /// <summary>
        /// Delete Old(zero) record
        /// </summary>
        void DeleteOldRecord();


        /// <summary>
        /// Delete All records
        /// </summary>
        void DeleteAllRecord();

        /// <summary>
        /// Should object rewind with others or he could continue move
        /// </summary>
        /// <returns></returns>
        bool ShouldRewind();

    }
}