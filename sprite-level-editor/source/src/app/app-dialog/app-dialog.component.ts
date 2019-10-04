import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { SpriteSheetInterface } from '../interfaces/sprite-sheet.interface';

@Component({
	selector: 'app-dialog',
	templateUrl: './app-dialog.component.html',
	styleUrls: ['./app-dialog.component.scss']
})
export class AppDialogComponent {

	public tileSize: number;

	private _dialogRef: MatDialogRef<AppDialogComponent, SpriteSheetInterface>
	private _imageBuffer: ArrayBuffer;
	private _image: HTMLImageElement;
	private _type: string;
	private _width: number;
	private _height: number;
	private _name: string;

	constructor(dialogRef: MatDialogRef<AppDialogComponent, SpriteSheetInterface>) {
		this._dialogRef = dialogRef;
	}

	public OnCloseClick() {
		this._dialogRef.close();
		this.tileSize = 0;
		this._imageBuffer = new ArrayBuffer(0);
	}

	public OnLoadClick() {
		if (this._image === undefined) this._dialogRef.close();

		this._dialogRef.close({
			tileSize: this.tileSize,
			imageBuffer: this._imageBuffer,
			image: this._image,
			type: this._type,
			width: this._width,
			height: this._height,
			name: this._name
		});
	}

	public OnFileInputChanged(event) {
		const reader = new FileReader();
		if (event.target.files && event.target.files.length > 0) {
			const file = event.target.files[0];

			const validImageFormats = [
				"image/jpeg",
				"image/png",
			]

			if (!validImageFormats.filter(e => e === file.type).length) return;

			this._type = file.type;
			this._name = file.name;
			reader.readAsDataURL(file);
			reader.onload = () => {
				this._imageBuffer = reader.result as ArrayBuffer;

				const img = new Image();
				img.onload = () => {
					this._width = img.width;
					this._height = img.height;
					this._image = img;
				}
				img.src = `data:${this._imageBuffer}`;
			};
		}
	}
}
