export interface SpriteSheetInterface {
    name: string;
    tileSize: number;
    imageBuffer: ArrayBuffer;
    image: HTMLImageElement;
    type: string;
    width: number;
    height: number;
}