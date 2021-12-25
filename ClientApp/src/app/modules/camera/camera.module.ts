import { HttpClientModule } from "@angular/common/http";
import { NgModule } from "@angular/core";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { TranslateModule } from "@ngx-translate/core";
import { InlineSVGModule } from "ng-inline-svg";
import { CameraCardComponent } from "./camera-card/camera-card.component"
import { CameraComponent } from "./camera.component";

@NgModule({
    declarations : [CameraComponent , CameraCardComponent] ,
    imports : [
        BrowserAnimationsModule,
        TranslateModule,
        HttpClientModule,
        InlineSVGModule.forRoot(),
    ] ,
    exports : [
        CameraComponent ,
        CameraCardComponent
    ]
})

export class CameraModule{}
