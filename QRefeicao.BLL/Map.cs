using Mapster;

namespace QRefeicao.BLL
{
    public static class Map<D>
    {
        public static D Convert<O>(O o)
        {
            if (o == null)
                return default(D);

            return o.Adapt<D>();
        }
    }
}
