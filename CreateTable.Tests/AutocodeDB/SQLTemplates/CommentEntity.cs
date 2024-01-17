namespace AutocodeDB.SQLTemplates
{
    public static class CommentEntity
    {
        ///"([\s;]|^)(/\*[\s\S]*?(\*/))";
        public const string Comments = @"\/\*[\s\S]*?\*\/|\-\-.*$";
    }
}
