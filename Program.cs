// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using HL7parser.dto;
using HL7parser.v3;

Console.WriteLine("Hello, World!");

decimal count = 1000;
string v3xml = File.ReadAllText("v3xmlfile/OrganizationInfoRegister.xml");

#region  hl7v3 parser example : use HL7v3Mapper
Stopwatch stopWatch = Stopwatch.StartNew();
HL7v3Mapper deptMapper = new HL7v3Mapper()
        .Add(nameof(DeptDto.DeptCode), DeptDto.DeptCode_Xpath)
        .Add(nameof(DeptDto.DeptName), DeptDto.DeptName_Xpath)
        .Add(nameof(DeptDto.DeptType), DeptDto.DeptType_Xpath, isRequired: false)
        .Add(nameof(DeptDto.ParentDeptCode), DeptDto.ParentDeptCode_Xpath, isRequired: false);
for (var i = 0; i < count; i++)
{
    HL7v3Parser v3Parser = new HL7v3Parser();
    string reqId = v3Parser.InitXmlDocument(v3xml);
    DeptDto deptDto = new DeptDto();
    v3Parser.DoMapper(deptDto, deptMapper);
}
var totalElapsed = stopWatch.ElapsedMilliseconds;
Console.WriteLine($"HL7v3Mapper total elapsed：{totalElapsed}ms");
Console.WriteLine($"HL7v3Mapper once elapsed：{totalElapsed / count}ms");
//Console.WriteLine($"reqid：{reqId}");
//Console.WriteLine(deptDto.ToString());
#endregion

#region  hl7v3 parser example : use HL7v3Attribute
var stopWatch2 = Stopwatch.StartNew();
for (var i = 0; i < count; i++)
{
    HL7v3Parser v3Parser2 = new HL7v3Parser();
    string reqId2 = v3Parser2.InitXmlDocument(v3xml);
    DeptDto deptDto2 = new DeptDto();
    v3Parser2.DoMapper(deptDto2);
}
var totalElapsed2 = stopWatch2.ElapsedMilliseconds;
Console.WriteLine($"HL7v3Attribute total elapsed：{totalElapsed2}ms");
Console.WriteLine($"HL7v3Attribute once elapsed：{totalElapsed2 / count}ms");
//Console.WriteLine($"reqid：{reqId2}");
//Console.WriteLine(deptDto2.ToString());
#endregion
