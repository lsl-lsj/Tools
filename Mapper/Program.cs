using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;
using AutoMapper;

namespace Mapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new Source
            {
                A = "A",
                B = DateTime.Now,
                C = Type.L,
                D = new List<Test>
                {
                    new Test{
                    test1 = "2",
                    test2 = DateTime.Now.ToString(),
                    test3 = new List<int> { 1, 2, 3 },
                    test4 = DateTime.Now,
                    test5 = Type.M
                    }
                }
            };

            IEnumerable<Source> list1 = new List<Source> { source };
            var list2 = list1.ListMapTo<Source, Destination>();
            Console.WriteLine(string.Join(",", list2));

            var des = source.ObjectMapTo<Source,Destination>();
            Console.WriteLine(des.ToString());
        }
    }

    public class Source
    {
        public string A { get; set; }
        public DateTime B { get; set; }
        public Type C { get; set; }

        public List<Test> D { get; set; }

        public override string ToString()
        {
            return A + B + C;
        }
    }

    public class Destination
    {
        public string A { get; set; }
        public string B { get; set; }
        public int C { get; set; }
        public List<Test> D { get; set; }

        public override string ToString()
        {
            return A + B + C + string.Join(",", D.Select(d => d.ToString()));
        }
    }

    public enum Type
    {
        L = 1,
        M = 2,
    }

    public class Test
    {
        public string test1 { get; set; }
        public string test2 { get; set; }
        public List<int> test3 { get; set; }
        public DateTime test4 { get; set; }
        public Type test5 { get; set; }

        public override string ToString()
        {
            return test1 + test2 + string.Join(",", test3) + test4 + test5;
        }
    }

    public static class CustomAuToMapper
    {
        public static TDestination ObjectMapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
        {
            if (source == null)
                return default(TDestination);

            var mapper = new MapperConfiguration(c =>
            {
                c.CreateMap<TSource, TDestination>();
            }).CreateMapper();
            return mapper.Map<TDestination>(source);
        }
        public static IEnumerable<TDestination> ListMapTo<TSource, TDestination>(this IEnumerable<TSource> sources)
            where TSource : class
            where TDestination : class
        {
            if (sources == null)
                return default(IEnumerable<TDestination>);

            var mapper = new MapperConfiguration(c =>
            {
                c.CreateMap<TSource, TDestination>();
            }).CreateMapper();
            return mapper.Map<IEnumerable<TDestination>>(sources);
        }
    }

}
