// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using HL7parser.dto;
using HL7parser.v3;
using HL7parser.v2;

Console.WriteLine("Hello, World!");

#region hl7v2 parser example
var msg = @"MSH|^~\&|ADT|ADI|ADT-1|ADI-1|20050215||ADT^A01|MSGADT003|T|2.4" + "\r" +
                             "EVN|A01|20031016000000" + "\r" +
                             "PID|1|111222333|H123123^^^^MR^ADT~111-222-333^^^^SS^ADT||John^Smith|GARSEN^^Melissa|19380818|M||2028-9|241 AVE^^Lake City^WA^98125^^^^100|100|(425)111-2222|(425)111-2222||S|CHR|1234567|111-222-333" + "\r" +
                             "NK1|2|GARSEN^Melissa" + "\r" +
                             "PV1|1|E|||||D123^Jeff^Carron|||MED||||7|||D123^Jeff^Taylor|E|3454|R^20050215|||||||||||||||||||EM|||||20050215" + "\r" +
                             "IN1|1|I123|ICOMP1|INS COMP 1|PO BOX 1^^Lake City^WA^98125||||||||||1|John^Smith|01|19380818" + "\r" +
                             "IN2|1||RETIRED" + "\r" +
                             "IN1|2|I456|ICOMP2|INS COMP 1|PO BOX 2^^Lake City^WA^98125||||||||||8|John^Smith|01|19380818" + "\r" +
                             "IN2|2||RETIRED" + "\r";
var hl7v2 = new Message(msg);
hl7v2.ParseMessage();
List<Segment> segList = hl7v2.Segments();
//获取PID里面的姓名
var patName = hl7v2.getValue("PID.5");
patName = hl7v2.DefaultSegment("PID").Fields(5).Value;
patName = hl7v2.Segments("PID")[0].Fields(5).Value;
var isComponentized = hl7v2.IsComponentized("PID.5");
#endregion

decimal count = 1000;
string v3xml = File.ReadAllText("v3xmlfile/OrganizationInfoRegister.xml");

#region  hl7v3 parser example : use HL7v3Mapper
Stopwatch stopWatch = Stopwatch.StartNew();
HL7v3Mapper deptMapper = new HL7v3Mapper()
        .Add(nameof(DeptDto.DeptCode), DeptDto.DeptCode_Xpath)
        .Add(nameof(DeptDto.DeptName), DeptDto.DeptName_Xpath)
        .Add(nameof(DeptDto.DeptType), DeptDto.DeptType_Xpath, isRequired: false)
        .Add(nameof(DeptDto.ParentDeptCode), DeptDto.ParentDeptCode_Xpath, isRequired: false, destType: MapType.TString
        , p =>
        {
            var output = DeptDto.HandlerData(p);
            return output;
        });
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
