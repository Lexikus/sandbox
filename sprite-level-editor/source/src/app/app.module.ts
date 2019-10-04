import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing/app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material-module';

import { AppComponent } from './app.component';
import { AppSidebarComponent } from './app-sidebar/app-sidebar.component';
import { AppDrawerComponent } from './app-drawer/app-drawer.component';
import { AppSettingsComponent } from './app-settings/app-settings.component';
import { AppDialogComponent } from './app-dialog/app-dialog.component';
import { AppBrushComponent } from './app-brush/app-brush.component';

import { PainterService } from './services/painter.service';

import { MatDialogRef } from '@angular/material/dialog';

@NgModule({
  declarations: [
    AppComponent,
    AppSidebarComponent,
    AppDrawerComponent,
    AppSettingsComponent,
    AppDialogComponent,
    AppBrushComponent
  ],
  entryComponents: [
    AppDialogComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FormsModule
  ],
  providers: [
    PainterService,
    { provide: MatDialogRef, useValue: [] }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
