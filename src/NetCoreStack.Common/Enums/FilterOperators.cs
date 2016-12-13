namespace NetCoreStack.Common
{
    public enum FilterLogicalOperator
    {
        And,
        Or
    }

    public enum FilterOperator
    {
        IsLessThan,
        IsLessThanOrEqualTo,
        IsEqualTo,
        IsNotEqualTo,
        IsGreaterThanOrEqualTo,
        IsGreaterThan,
        StartsWith,
        EndsWith,
        Contains,
        IsContainedIn,
        DoesNotContain,
        IsNull,
        IsNotNull,
        IsEmpty,
        IsNotEmpty,
        In
    }
}
