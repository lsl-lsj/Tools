using System;
using System.ComponentModel;
namespace EBook.Domain
{
    public class EbookInfo
    {
        [Description("Id")]
        public string Id { get; set; }

        [Description("编号")]
        public int Number { get; set; }

        [Description("名称")]
        public string Name { get; set; }

        [Description("类型")]
        public BookType Type { get; set; }

        [Description("作者")]
        public string Author { get; set; }

        [Description("出版社")]
        public string Publish { get; set; }

        [Description("出版日期")]
        public DateTime PublishDate { get; set; }

        [Description("价格")]
        public double Price { get; set; }

        [Description("积分价格")]
        public int Score { get; set; }

        [Description("下载总次数")]
        public int DownloadTimes { get; set; }

        [Description("简介")]
        public string Description { get; set; }

        [Description("是否打折")]
        public bool IsDiscount { get; set; }

        [Description("折扣")]
        public double Discount { get; set; }

        [Description("是否删除")]
        public bool IsDeleted { get; set; }

        [Description("上传目录")]
        public string Url { get; set; }

        [Description("实际价格")]
        public double ActualPrice => IsDiscount ? Price * Discount : Price;
    }
    public enum BookType
    {
        [Description("网页制作")]
        Web = 1,

        [Description("编程开发")]
        Programming = 2,

        [Description("网络安全")]
        CyberSecurity = 3,

        [Description("图形图像")]
        GraphImage = 4,
    }
}