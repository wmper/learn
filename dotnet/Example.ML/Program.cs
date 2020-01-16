using Microsoft.ML;
using Microsoft.ML.Data;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;

namespace Example.ML
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var dbFactory = new OrmLiteConnectionFactory("Server=127.0.0.1;Port=3306;Database=cherry;Uid=root;Pwd=;SslMode=None;", MySqlDialect.Provider);

            var list = new List<SSQDto>();
            using (var db = dbFactory.Open())
            {
                var query = db.From<SSQ>().Where(x => x.No > 19000 && x.No < 20003);

                list = db.Select<SSQDto>(query);
            }

            MLContext mlContext = new MLContext();

            IDataView trainingData = mlContext.Data.LoadFromEnumerable(list.ToArray());

            // 2. Specify data preparation and model training pipeline
            var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "H1" });

            pipeline.Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "H2", maximumNumberOfIterations: 100));

            // 3. Train model
            var model = pipeline.Fit(trainingData);

            // 4. Make a prediction
            var input = new SSQDto() { H1 = 9 };

            var rs = mlContext.Model.CreatePredictionEngine<SSQDto, Prediction>(model).Predict(input);

            Console.WriteLine($"Predicted total : {rs.H2}");

            Console.Read();
        }
    }

    public class Prediction
    {
        [ColumnName("Score")]
        public int H2 { get; set; }
    }
}
