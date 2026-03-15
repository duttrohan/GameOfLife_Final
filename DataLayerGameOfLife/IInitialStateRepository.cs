using System.Collections.Generic;

namespace DataLayerGameOfLife
{
    public interface IInitialStateRepository
    {
        // Matches UML: +Get(name:string): InitialState
        List<(int x, int y)> Get(string name);

        // Matches UML: +Add(initialState: InitialState): void
        void Add(string name, List<(int x, int y)> aliveCells);

        // Required to satisfy the full UML design
        void Update(string name, List<(int x, int y)> cells);
        void Delete(string name);
    }
}