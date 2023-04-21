// See https://aka.ms/new-console-template for more information
using HL7parser.dto;
using HL7parser.v3;

Console.WriteLine("Hello, World!");

#region  hl7v3 parser example
string v3xml = File.ReadAllText("v3xmlfile/OrganizationInfoRegister.xml");
const string ParentDeptCode_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/assignedPrincipalOrganization/asAffiliate/affiliatedPrincipalOrganization/id/item[@root='2.16.156.10011.1.26']/@extension";
const string DeptCode_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/id/item[@root='2.16.156.10011.1.26']/@extension";
const string DeptName_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/assignedPrincipalOrganization/name/item/part/@value";
const string DeptType_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/code/translation/translation/@value";
HL7v3Mapper deptMapper = new HL7v3Mapper()
   .Add(nameof(DeptDto.DeptCode), DeptCode_Xpath)
   .Add(nameof(DeptDto.DeptName), DeptName_Xpath)
   .Add(nameof(DeptDto.DeptType), DeptType_Xpath, isRequired: false)
   .Add(nameof(DeptDto.ParentDeptCode), ParentDeptCode_Xpath, isRequired: false);
HL7v3Parser v3Parser = new HL7v3Parser();
string reqId = v3Parser.InitXmlDocument(v3xml);
Console.WriteLine($"reqid：{reqId}");
DeptDto deptDto = new DeptDto();
v3Parser.DoMapper<DeptDto>(deptDto, deptMapper);
Console.WriteLine(deptDto.ToString());
#endregion
