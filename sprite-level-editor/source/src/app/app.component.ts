import { Component } from '@angular/core';
import { PainterService } from './services/painter.service';
import { saveAs } from "file-saver";

@Component({
	selector: 'app-root',
	templateUrl: './app.component.html',
	styleUrls: ['./app.component.scss']
})
export class AppComponent {
	private _painter;

	public constructor(painter: PainterService) {
		this._painter = painter;
	}

	public OnSaveClick() {
		const json = {
			settings: this._painter.GetGridSettings(),
			images: this._painter.GetImageInformations(),
			grid: this._painter.GetGridInformation()
		};
		const blob = new Blob([JSON.stringify(json)], { type: "application/json" });
		saveAs(blob, "level.json");
	}

	public OnTileSheetFileInputChanged(event) {
		const reader = new FileReader();
		if (event.target.files && event.target.files.length > 0) {
			const file = event.target.files[0];
			if (file.type !== "application/json") return;

			reader.readAsText(file);
			reader.onload = async () => {
				try {
					const jsonObject = JSON.parse(reader.result as string);
					this._painter.SetGridSettings(jsonObject.settings);
					await this._painter.SetImageInformations(jsonObject.images);
					this._painter.SetGridInformation(jsonObject.grid);
				} catch (error) {
					console.error(error);
				}
			}
		}
	}
}
