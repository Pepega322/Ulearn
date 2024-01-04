using System.Linq;
using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier
{
    public string GetApiDescription()
    {
        var attribute = typeof(T).GetCustomAttribute<ApiDescriptionAttribute>();
        return (attribute != null) ? attribute.Description : null;
    }

    public string[] GetApiMethodNames()
    {
        return typeof(T).GetMethods()
            .Where(m => m.GetCustomAttribute<ApiMethodAttribute>() != null)
            .Select(m => m.Name)
            .ToArray();
    }

    public string GetApiMethodDescription(string methodName)
    {
        var method = typeof(T).GetMethod(methodName);
        if (method == null) return null;
        var attribute = method.GetCustomAttribute<ApiDescriptionAttribute>();
        return attribute != null ? attribute.Description : null;
    }

    public string[] GetApiMethodParamNames(string methodName)
    {
        var method = typeof(T).GetMethod(methodName);
        if (method == null) return null;
        return method.GetParameters().Select(p => p.Name).ToArray();
    }

    public string GetApiMethodParamDescription(string methodName, string paramName)
    {
        var method = typeof(T).GetMethod(methodName);
        if (method == null) return null;
        var param = method.GetParameters().Where(p => p.Name == paramName).FirstOrDefault();
        if (param == null) return null;
        var attribute = param.GetCustomAttribute<ApiDescriptionAttribute>();
        if (attribute == null) return null;
        return attribute.Description;
    }

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
    {
        var tempDiscription = new ApiParamDescription();
        tempDiscription.ParamDescription = new CommonDescription(paramName);
        var method = typeof(T).GetMethod(methodName);
        if (method == null || method.GetCustomAttribute<ApiMethodAttribute>() == null) return tempDiscription;
        var param = method.GetParameters().Where(p => p.Name == paramName).FirstOrDefault();
        if (param == null) return tempDiscription;
        return GetParamDescription(param);
    }

    private ApiParamDescription GetParamDescription(ParameterInfo param)
    {
        var description = new ApiParamDescription();
        description.ParamDescription = new CommonDescription(param.Name);
        var req = param.GetCustomAttribute<ApiRequiredAttribute>();
        if (req == null) return null;
        description.Required = req.Required;
        var desk = param.GetCustomAttribute<ApiDescriptionAttribute>();
        description.ParamDescription.Description = desk != null ? desk.Description : null;
        var minMax = param.GetCustomAttribute<ApiIntValidationAttribute>();
        description.MinValue = minMax?.MinValue;
        description.MaxValue = minMax?.MaxValue;
        return description;
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName)
    {
        var description = new ApiMethodDescription();
        description.MethodDescription = new CommonDescription(methodName);
        var method = typeof(T).GetMethod(methodName);
        if (method == null || method.GetCustomAttribute<ApiMethodAttribute>() == null) return null;
        description.ParamDescriptions = method.GetParameters()
            .Select(p => GetParamDescription(p))
            .ToArray();
        var desk = method.GetCustomAttribute<ApiDescriptionAttribute>();
        description.MethodDescription.Description = desk != null ? desk.Description : null;
        description.ReturnDescription = GetParamDescription(method.ReturnParameter);

        return description;
    }
}
