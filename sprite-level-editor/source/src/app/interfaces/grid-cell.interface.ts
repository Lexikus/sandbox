import { GridEmptyCellInterface } from "./grid-empty-cell.interface";

export interface GridCellInterface extends GridEmptyCellInterface {
    spriteX: number,
    spriteY: number,
    spriteTileSize: number,
    sprite: CanvasImageSource
}