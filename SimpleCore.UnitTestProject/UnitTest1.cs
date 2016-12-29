using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCore;
using SimpleCore.Entity;
using System.Linq;
using System.ComponentModel;
using System.Reflection;

namespace SimpleCore.UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            //*
            //1、app.config或者web.config配置链接字符串及Sql Provider（有因Type.GetType()无法忽略dll版本号，需修改PetaPoco.cs中的dll版本号）
            //2、配置Entity.tt生成实体
            //3、重新生成项目dll，配置Business.tt生成业务层
            //4、重新生成项目dll，配置Partial.tt生成部分类（部分类未增加业务代码，有新写入代码后切勿重新此操作）
            //4-、可配置实体参数CRUD，配置Partial.tt中实体参数输出命名空间生成实体参数类，实体参数类中加入属性参数（属性需加[DisplayName("filed")]特性标签才被视为有效参数，filed为真实数据库表中字段），重新生成项目dll后，配置Business.tt实体参数所在命名空间重新生成业务层 即有实体参数CRUD
        }
    }
}
