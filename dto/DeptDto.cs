using HL7parser.v3;
namespace HL7parser.dto
{
    public class DeptDto
    {
        public const string ParentDeptCode_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/assignedPrincipalOrganization/asAffiliate/affiliatedPrincipalOrganization/id/item[@root='2.16.156.10011.1.26']/@extension";
        public const string DeptCode_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/id/item[@root='2.16.156.10011.1.26']/@extension";
        public const string DeptName_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/assignedPrincipalOrganization/name/item/part/@value";
        public const string DeptType_Xpath = "/controlActProcess/subject/registrationRequest/subject1/assignedEntity/code/translation/translation/@value";

        [HL7v3(xpath: DeptCode_Xpath)]
        public string DeptCode { get; set; }
        [HL7v3(xpath: DeptName_Xpath)]
        public string DeptName { get; set; }
        [HL7v3(xpath: DeptType_Xpath, isRequired: false)]
        public string DeptType { get; set; }
        [HL7v3(xpath: ParentDeptCode_Xpath, isRequired: false]
        public string ParentDeptCode { get; set; }
        public override string ToString()
        {
            return $"deptCode:{DeptCode},deptName:{DeptName},deptType:{DeptType},parentDeptCode:{ParentDeptCode}";
        }

        public static string HandlerData(string input)
        {
            return "哈哈哈" + input + "哦哦哦";
        }
    }

}