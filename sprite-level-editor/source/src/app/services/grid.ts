export class Grid {
    private readonly _cap: number;
    private _grid: [{}] | [{ x: number, y: number, sprite: string }][][] = [];

    public constructor(cap: number) {
        this._cap = cap;

        for (let i = 0; i < cap; i++) {
            this._grid[i] = [];
            for (let j = 0; j < cap; j++) {
                this._grid[i][j] = [];
            }
        }
    }

    public Add(gridX: number, gridY: number, x: number, y: number, sprite: string) {
        this._grid[gridX][gridY].push({ x, y, sprite });
    }

    public Contains(gridX: number, gridY: number, x: number, y: number, sprite: string): boolean {
        return this._grid[gridX][gridY].filter(e => e.x === x && e.y === y && e.sprite === sprite).length;
    }

    public Remove(gridX: number, gridY: number) {
        this._grid[gridX][gridY] = [];
    }

    public Get(gridX: number, gridY: number): [{ x: number, y: number, sprite: string }] {
        return this._grid[gridX][gridY];
    }
}