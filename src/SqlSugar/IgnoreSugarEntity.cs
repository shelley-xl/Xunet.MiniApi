// THIS FILE IS PART OF Xunet.MiniApi PROJECT
// THE Xunet.MiniApi PROJECT IS AN OPENSOURCE LIBRARY LICENSED UNDER THE MIT License.
// COPYRIGHTS (C) 徐来 ALL RIGHTS RESERVED.
// GITHUB: https://github.com/shelley-xl/Xunet.MiniApi

namespace Xunet.MiniApi.SqlSugar;

/// <summary>
/// 忽略SugarEntity，CodeFirst时将不会生成表
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreSugarEntity : Attribute
{

}
