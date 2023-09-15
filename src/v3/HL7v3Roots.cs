namespace HL7parser.v3
{
    public class HL7v3Roots
    {
        /// <summary>
        /// 医生标识
        /// </summary>
        const string Root_EmplCode = "2.16.156.10011.1.4";
        /// <summary>
        /// 科室标识
        /// </summary>
        const string Root_DeptCode = "2.16.156.10011.1.26";
        /// <summary>
        /// 病区标识
        /// </summary>
        const string Root_AreaCode = "2.16.156.10011.1.27";
        /// <summary>
        /// 患者标识
        /// </summary>
        const string Root_PatientNo = "2.16.156.10011.2.5.1.4";
        /// <summary>
        /// 住院号标识
        /// </summary>
        const string Root_InpatId = "2.16.156.10011.1.12";
        /// <summary>
        /// 就诊流水号标识
        /// </summary>
        const string Root_HisSerialNumber = "2.16.156.10011.2.5.1.9";
    }
}