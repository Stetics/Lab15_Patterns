using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using System;


var data = new MyLogger();
data.AddData(new MyData { Id = 7, Name = "Jack" });
data.AddData(new MyData { Id = 15, Name = "Ivan" });

int i = 0;
Console.WriteLine("choose 1 or 2:");
i = Convert.ToInt32(Console.ReadLine());
if(i == 1)
{
    data.Write("json");
}
else if(i == 2)
{
    data.Write("txt");
}

public class MyLogger: IRepository<MyData>
{
    List<MyData> data = new();
    public void AddData(MyData d)
        { data.Add(d); }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public void Delete(int id)
    {
        this.data.Remove(this.data.Where(x => x.Id == id).ToList()[0]);
    }

    public void Write(string form)
    {
        if (form == "json")
        {
            using (var writer = new StreamWriter(@"C:\Users\user\source\repos\Lab_15\TASK2\bin\Debug\net6.0\LB15.json", true))
            {
                foreach (var d in data)
                {
                    string jsonString = JsonSerializer.Serialize(d);
                    writer.WriteLine(jsonString);
                }
            }
        }
        else if(form == "txt")
        {
            using(var writer = new StreamWriter(@"C:\Users\user\source\repos\Lab_15\TASK2\bin\Debug\net6.0\LB15.txt", true))
            {
                foreach(var d in data)
                {
                    writer.WriteLine($"{d.Id} + {d.Name}");
                }
            }
        }
    }
}


interface IRepository<T> : IDisposable where T : class
{
    public void AddData(T a);
    public void Write(string form);
    public void Delete(int id);

}


public class MyData
{
    public int Id { get; set; }

    public string? Name { get; set; }
}

