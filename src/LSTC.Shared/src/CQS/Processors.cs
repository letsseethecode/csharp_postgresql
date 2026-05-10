using System.ComponentModel.DataAnnotations;

public abstract class Processor
{
    protected void ValidateObject(object value)
    {
        var context = new ValidationContext(value);
        var results = new List<ValidationResult>();
        if (!Validator.TryValidateObject(value, context, results, true))
        {
            var e = results.GetEnumerator();
            if (e.MoveNext())
            {
                var first = e.Current;
                if (!e.MoveNext())
                    throw new ValidationException(first, null, value);
                else
                    throw new AggregateException(
                        "Validation failed",
                        results.Select(r => new ValidationException(r, null, value))
                    );

            }
        }
    }
}