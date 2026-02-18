namespace Common.Entities
{
    public class WorkEntity
    {
        //TODO make workEntity contain keys: work type (carrying, building, surveying), message, and data
        //Carrying just prints message to the console
        //Surveying pauses worker as it is surveying the work, can't block thread, use tasks & await to turn control back to controller
        //Build chooses number from 1 to 5, how many steps it takes to do the build, 1st dequeue the message, decrement by 1, 
        // requeue the message back into the queue for the same worker to process the message, when it gets to 0, delete the message and done

        public string? WorkType { get; set; }

        public string? Message { get; set; }

        public string? Data { get; set; }

        public override string ToString()
        {
            return WorkType + " " + Message + " " + Data;
        }
    }
}
