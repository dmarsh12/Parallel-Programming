using System;
namespace Parallel_Programming
{
    public class TaskBasics
    {
        public TaskBasics()
        {
        }

        /*A Task is a unit of work*/
        public void Write(char c)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(c);
            }
        }

        public void WriteObject(object o)
        {
            int i = 1000;
            while (i-- > 0)
            {
                Console.Write(o);
            }
        }

        public int TextLength(object o)
        {
            Console.WriteLine($"Thread {Task.CurrentId} processing object {o}....");
            return o.ToString().Length;
        }


        public void StartTaskOptionA()
        {
            /*option A to run a task...
             * you not only create one
             * but you also start running 
             * it at creation...*/
            Task.Factory.StartNew(() => Write('.')); //OWN THREAD

            /*option B to run a task...
             * you create one then
             * explicitly define when to run
             * it...*/
            var writeTask = new Task(() => Write('?')); //OWN THREAD
            writeTask.Start();

            Write('-'); //MAIN THREAD
        }

        /*option B requires passing the parameter
        * as an object to the method...*/
        public void StartTaskOptionB()
        {
            var writeTask = new Task(WriteObject, "Hello"); //Hello is used as a parameter here....
            writeTask.Start();

            Task.Factory.StartNew(WriteObject, 123);
        }

        public void Run()
        {
            //StartTaskOptionA();

            //StartTaskOptionB();

            string text1 = "testing", text2 = "different";
            Task<int> taskOne = new Task<int>(TextLength, text1);
            taskOne.Start();

            Task<int> taskTwo = Task.Factory.StartNew(TextLength, text2);

            Console.WriteLine($"Length of '{text1}' is {taskOne.Result}");
            Console.WriteLine($"Length of '{text2}' is {taskTwo.Result}");

            Console.WriteLine("Program done");
            Console.ReadKey();
        }
    }
}

