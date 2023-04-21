namespace HL7parser.dto
{
    public class DeptDto
    {
        public string DeptCode { get; set; }
        public string DeptName { get; set; }
        public string DeptType { get; set; }
        public string ParentDeptCode { get; set; }
        public override string ToString()
        {
            return $"deptCode:{DeptCode},deptName:{DeptName},deptType:{DeptType},parentDeptCode:{ParentDeptCode}";
        }
    }
}