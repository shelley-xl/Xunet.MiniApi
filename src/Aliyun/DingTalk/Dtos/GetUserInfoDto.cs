// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.Aliyun.DingTalk.Dtos;

/// <summary>
/// 获取用户信息返回
/// </summary>
public class GetUserInfoDto : ErrorDto
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [JsonPropertyName("result")]
    public ResultObject? Result { get; set; }

    /// <summary>
    /// 返回结果
    /// </summary>
    public class ResultObject
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [JsonPropertyName("userid")]
        public string? UserId { get; set; }

        /// <summary>
        /// 用户UnionId
        /// </summary>
        [JsonPropertyName("unionid")]
        public string? UnionId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// 头像（员工使用默认头像，不返回该字段，手动设置头像会返回）
        /// </summary>
        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

        /// <summary>
        /// 国际电话区号（第三方企业应用不返回该字段；如需获取state_code，可以使用钉钉统一授权套件方式获取）
        /// </summary>
        [JsonPropertyName("state_code")]
        public string? StateCode { get; set; }

        /// <summary>
        /// 员工的直属主管（员工在企业管理后台个人信息面板中，直属主管内有值，才会返回该字段）
        /// </summary>
        [JsonPropertyName("manager_userid")]
        public string? ManagerUserId { get; set; }

        /// <summary>
        /// 手机号（企业内部应用，只有应用开通通讯录企业员工手机号信息权限，才会返回该字段。第三方企业应用不返回该字段，如需获取mobile，可以使用钉钉统一授权套件方式获取）
        /// </summary>
        [JsonPropertyName("mobile")]
        public string? Mobile { get; set; }

        /// <summary>
        /// 是否号码隐藏
        /// </summary>
        [JsonPropertyName("hide_mobile")]
        public bool? HideMobile { get; set; }

        /// <summary>
        /// 分机号（第三方企业应用不返回该参数）
        /// </summary>
        [JsonPropertyName("telephone")]
        public string? Telephone { get; set; }

        /// <summary>
        /// 员工工号
        /// </summary>
        [JsonPropertyName("job_number")]
        public string? JobNumber { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// 员工邮箱
        /// </summary>
        [JsonPropertyName("email")]
        public string? Email { get; set; }

        /// <summary>
        /// 办公地点
        /// </summary>
        [JsonPropertyName("work_place")]
        public string? WorkPlace { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonPropertyName("remark")]
        public string? Remark { get; set; }

        /// <summary>
        /// 是否为企业账号
        /// </summary>
        [JsonPropertyName("exclusive_account")]
        public bool? ExclusiveAccount { get; set; }

        /// <summary>
        /// 员工的企业邮箱（如果员工的企业邮箱没有开通，返回信息中不包含该数据）
        /// </summary>
        [JsonPropertyName("org_email")]
        public string? OrgEmail { get; set; }

        /// <summary>
        /// 所属部门Id列表
        /// </summary>
        [JsonPropertyName("dept_id_list")]
        public int[]? DeptIdList { get; set; }

        /// <summary>
        /// 员工在对应的部门中的排序
        /// </summary>
        [JsonPropertyName("dept_order_list")]
        public DeptOrderListObject[]? DeptOrderList { get; set; }

        /// <summary>
        /// 扩展属性，最大长度2000个字符
        /// </summary>
        [JsonPropertyName("extension")]
        public string? Extension { get; set; }

        /// <summary>
        /// 入职时间，Unix时间戳，单位毫秒
        /// </summary>
        [JsonPropertyName("hired_date")]
        public long? HiredDate { get; set; }

        /// <summary>
        /// 是否激活了钉钉
        /// </summary>
        [JsonPropertyName("active")]
        public bool? Active { get; set; }

        /// <summary>
        /// 是否完成了实名认证
        /// </summary>
        [JsonPropertyName("real_authed")]
        public bool? RealAuthed { get; set; }

        /// <summary>
        /// 是否为企业的高管
        /// </summary>
        [JsonPropertyName("senior")]
        public bool? Senior { get; set; }

        /// <summary>
        /// 是否为企业的管理员
        /// </summary>
        [JsonPropertyName("admin")]
        public bool? Admin { get; set; }

        /// <summary>
        /// 是否为企业的老板
        /// </summary>
        [JsonPropertyName("boss")]
        public bool? Boss { get; set; }

        /// <summary>
        /// 员工所在部门信息及是否是领导
        /// </summary>
        [JsonPropertyName("leader_in_dept")]
        public LeaderInDeptObject[]? LeaderInDept { get; set; }

        /// <summary>
        /// 角色列表
        /// </summary>
        [JsonPropertyName("role_list")]
        public RoleListObject[]? RoleList { get; set; }

        /// <summary>
        /// 员工在对应的部门中的排序
        /// </summary>
        public class DeptOrderListObject
        {
            /// <summary>
            /// 部门Id
            /// </summary>
            [JsonPropertyName("dept_id")]
            public int? DeptId { get; set; }

            /// <summary>
            /// 部门排序
            /// </summary>
            [JsonPropertyName("order")]
            public long? Order { get; set; }
        }

        /// <summary>
        /// 员工所在部门信息及是否是领导
        /// </summary>
        public class LeaderInDeptObject
        {
            /// <summary>
            /// 部门Id
            /// </summary>
            [JsonPropertyName("dept_id")]
            public int? DeptId { get; set; }

            /// <summary>
            /// 是否是领导
            /// </summary>
            [JsonPropertyName("leader")]
            public bool? Leader { get; set; }
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        public class RoleListObject
        {
            /// <summary>
            /// 角色Id
            /// </summary>
            [JsonPropertyName("id")]
            public int? Id { get; set; }

            /// <summary>
            /// 角色名称
            /// </summary>
            [JsonPropertyName("name")]
            public string? Name { get; set; }

            /// <summary>
            /// 角色组名称
            /// </summary>
            [JsonPropertyName("group_name")]
            public string? GroupName { get; set; }
        }
    }
}
