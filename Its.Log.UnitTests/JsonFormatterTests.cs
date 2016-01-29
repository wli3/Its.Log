using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Its.Log.Instrumentation.UnitTests
{
    [TestFixture]
    public class JsonFormatterTests
    {
        [Test]
        public void Json_formatter_should_convert_plain_nothing_object_to_standard_json()
        {
            var sut = new JsonFormatter();
            var emptyObject = new object();

            var standardJson = JsonConvert.SerializeObject(emptyObject);

            var result = sut.Format(emptyObject);
            Assert.AreEqual(standardJson, result);
        }

        [Test]
        public void Json_formatter_should_convert_int_to_standard_json()
        {
            var sut = new JsonFormatter();
            var intToFormat = 42;

            var standardJson = JsonConvert.SerializeObject(intToFormat);

            var result = sut.Format(intToFormat);
            Assert.AreEqual(standardJson, result);
        }

        [Test]
        public void Json_formatter_should_convert_string_to_standard_json()
        {
            var sut = new JsonFormatter();
            var stringToFormat = "42";

            var standardJson = JsonConvert.SerializeObject(stringToFormat);

            var result = sut.Format(stringToFormat);
            Assert.AreEqual(standardJson, result);
        }

        [Test]
        public void Json_formatter_should_convert_at_limit_to_standard_without_truncation_json()
        {
           var sut = new JsonFormatter
           {
                ListExpansionLimit = 4
            };
            var underlimitList  = new List<int>() {4, 3, 2, 1};

            var standardJson = JsonConvert.SerializeObject(underlimitList);

            var result = sut.Format(underlimitList);
            Assert.AreEqual(standardJson, result);
        }

        [Test]
        public void Json_formatter_should_convert_anonymous_object_to_json()
        {
           var sut = new JsonFormatter
           {
                ListExpansionLimit = 4
            };
            var data = new
            {
                Foo = "bar"
            };

            var standardJson = JsonConvert.SerializeObject(data);

            var result = sut.Format(data);
            Assert.AreEqual(standardJson, result);
        }

        [Test]
        public void Json_formatter_should_convert_over_limit_to_standard_with_truncation_json()
        {
            var sut = new JsonFormatter
            {
                ListExpansionLimit = 4
            };
            var underlimitList = new List<int>() {5, 4, 3, 2, 1 };

            var standardJson = JsonConvert.SerializeObject(underlimitList.Take(4));

            var result = sut.Format(underlimitList);
            Assert.AreEqual(standardJson, result);
        }

    }

    public class JsonFormatter
    {
        public string Format(object obj)
        {
            Formatter.CreateWriter = () => new JsonStringWriter();
            return Formatter.Format(obj);
        }

        public int ListExpansionLimit { get; set; }
    }

    public class JsonStringWriter  : StringWriter
    {
        public override void Close()
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            base.Flush();
        }

        public override void Write(char value)
        {
            base.Write(value);
        }

        public override void Write(char[] buffer)
        {
            base.Write(buffer);
        }

        public override void Write(char[] buffer, int index, int count)
        {
            base.Write(buffer, index, count);
        }

        public override void Write(bool value)
        {
            base.Write(value);
        }

        public override void Write(int value)
        {
            base.Write(value);
        }

        public override void Write(uint value)
        {
            base.Write(value);
        }

        public override void Write(long value)
        {
            base.Write(value);
        }

        public override void Write(ulong value)
        {
            base.Write(value);
        }

        public override void Write(float value)
        {
            base.Write(value);
        }

        public override void Write(double value)
        {
            base.Write(value);
        }

        public override void Write(decimal value)
        {
            base.Write(value);
        }

        public override void Write(string value)
        {
            base.Write(value);
        }

        public override void Write(object value)
        {
            base.Write(JsonConvert.SerializeObject(value));
        }

        public override void Write(string format, object arg0)
        {
            base.Write(format, arg0);
        }

        public override void Write(string format, object arg0, object arg1)
        {
            base.Write(format, arg0, arg1);
        }

        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            base.Write(format, arg0, arg1, arg2);
        }

        public override void Write(string format, params object[] arg)
        {
            base.Write(format, arg);
        }

        public override void WriteLine()
        {
            base.WriteLine();
        }

        public override void WriteLine(char value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(char[] buffer)
        {
            base.WriteLine(buffer);
        }

        public override void WriteLine(char[] buffer, int index, int count)
        {
            base.WriteLine(buffer, index, count);
        }

        public override void WriteLine(bool value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(int value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(uint value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(long value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(ulong value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(float value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(double value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(decimal value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(string value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(object value)
        {
            base.WriteLine(value);
        }

        public override void WriteLine(string format, object arg0)
        {
            base.WriteLine(format, arg0);
        }

        public override void WriteLine(string format, object arg0, object arg1)
        {
            base.WriteLine(format, arg0, arg1);
        }

        public override void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            base.WriteLine(format, arg0, arg1, arg2);
        }

        public override void WriteLine(string format, params object[] arg)
        {
            base.WriteLine(format, arg);
        }

        public override Task WriteAsync(char value)
        {
            return base.WriteAsync(value);
        }

        public override Task WriteAsync(string value)
        {
            return base.WriteAsync(value);
        }

        public override Task WriteAsync(char[] buffer, int index, int count)
        {
            return base.WriteAsync(buffer, index, count);
        }

        public override Task WriteLineAsync(char value)
        {
            return base.WriteLineAsync(value);
        }

        public override Task WriteLineAsync(string value)
        {
            return base.WriteLineAsync(value);
        }

        public override Task WriteLineAsync(char[] buffer, int index, int count)
        {
            return base.WriteLineAsync(buffer, index, count);
        }

        public override Task WriteLineAsync()
        {
            return base.WriteLineAsync();
        }

        public override Task FlushAsync()
        {
            return base.FlushAsync();
        }

        public override IFormatProvider FormatProvider { get; }
        public override Encoding Encoding { get; }
        public override string NewLine { get; set; }
        public override object InitializeLifetimeService()
        {
            return base.InitializeLifetimeService();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return base.CreateObjRef(requestedType);
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}