using System.Reflection;

namespace Documentation;

public class Specifier<T> : ISpecifier {
    public string GetApiDescription()
        => typeof(T).GetCustomAttribute<ApiDescriptionAttribute>()
        ?.Description;

    public string[] GetApiMethodNames()
        => typeof(T).GetMethods()
            .Where(m => m.GetCustomAttribute<ApiMethodAttribute>() != null)
            .Select(m => m.Name)
            .ToArray();

    public string GetApiMethodDescription(string methodName)
        => typeof(T).GetMethod(methodName)
            ?.GetCustomAttribute<ApiDescriptionAttribute>()
            ?.Description;

    public string[] GetApiMethodParamNames(string methodName)
        => typeof(T).GetMethod(methodName)
            ?.GetParameters()
            .Select(p => p.Name)
            .ToArray();

    public string GetApiMethodParamDescription(string methodName, string paramName)
        => typeof(T).GetMethod(methodName)
            ?.GetParameters()
            .Where(p => p.Name == paramName)
            .FirstOrDefault()
            ?.GetCustomAttribute<ApiDescriptionAttribute>()
            ?.Description;

    public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName) {
        var discription = new ApiParamDescription();
        discription.ParamDescription = new CommonDescription(paramName);
        var paramInfo = typeof(T).GetMethod(methodName)
            ?.GetParameters()
            .Where(p => p.Name == paramName)
            .FirstOrDefault();
        return (paramInfo == null) ? discription : GetParamDescription(paramInfo);
    }

    private ApiParamDescription GetParamDescription(ParameterInfo param) {
        var description = new ApiParamDescription();
        var requiredAttribute = param.GetCustomAttribute<ApiRequiredAttribute>();
        var valueValidationAttribute = param.GetCustomAttribute<ApiIntValidationAttribute>();
        var descriptionAttribute = param.GetCustomAttribute<ApiDescriptionAttribute>();
        var paramName = param.Name != string.Empty ? param.Name : null;

        if (requiredAttribute != null) description.Required = requiredAttribute.Required;
        description.ParamDescription = new CommonDescription(paramName, descriptionAttribute?.Description);
        description.MinValue = valueValidationAttribute?.MinValue;
        description.MaxValue = valueValidationAttribute?.MaxValue;
        return description;
    }

    public ApiMethodDescription GetApiMethodFullDescription(string methodName) {
        var description = new ApiMethodDescription();
        var methodInfo = typeof(T).GetMethod(methodName);
        if (methodInfo == null || methodInfo.GetCustomAttribute<ApiMethodAttribute>() == null) return null;
        description.ParamDescriptions = methodInfo.GetParameters()
            .Select(p => GetParamDescription(p))
            .ToArray();

        var descriptionAttribute = methodInfo.GetCustomAttribute<ApiDescriptionAttribute>();
        description.MethodDescription = new CommonDescription(methodName, descriptionAttribute?.Description);
        if (methodInfo.ReturnType != typeof(void))
            description.ReturnDescription = GetParamDescription(methodInfo.ReturnParameter);

        return description;
    }
}
