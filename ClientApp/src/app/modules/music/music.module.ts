import { BluetoothPairComponent } from './bluetoothPair/bluetooth-pair.component';
import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { TranslateModule } from "@ngx-translate/core";
import { InlineSVGModule } from "ng-inline-svg";
import { MusicComponent } from "./music.component";

@NgModule({
    declarations : [
      MusicComponent,
      BluetoothPairComponent
    ] ,
    imports : [
        BrowserAnimationsModule,
        TranslateModule,
        HttpClientModule,
        InlineSVGModule.forRoot(),
    ] ,
    exports : [
      MusicComponent
    ]
})

export class MusicModule{}
