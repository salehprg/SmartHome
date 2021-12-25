import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { TranslateModule } from "@ngx-translate/core";
import { InlineSVGModule } from "ng-inline-svg";
import { GeneralModule } from "src/app/partials/general/general.module";
import { ActiveSessionsComponent } from "./active-sessions/active-sessions.component";
import { ModulesSettingsComponent } from "./modules-settings/modules-settings.component";
import { GeneralSettingsComponent } from "./general-settings/general-settings.component";
import { GeneralSettingCard } from "./general-settings/gs-card/gs-card.component";
import { HomeLockupComponent } from "./home-settings/home-lockup/home-lockup.component";
import { HomeSettingsComponent } from "./home-settings/home-settings.component";
import { ScenarioSettingsComponent } from "./scenario-settings/scenario-settings.component";
import { AddCardComponent } from "./setting-card/add-card/add-card.component";
import { SettingCardComponent } from "./setting-card/setting-card.component";
import { SettingFormComponent } from "./setting-form/setting-form.component";
import { SettingsHeaderComponent } from "./settings-header/settings-header.component";
import { SettingsComponent } from "./settings.component";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { CamerasSettingsComponent } from "./cameras-settings/cameras-settings.component";
import { CameraSettingForm } from "./cameras-settings/form/camera-form.component";

@NgModule({
    declarations : [
        SettingsComponent ,
        SettingsHeaderComponent,
        HomeSettingsComponent ,
        HomeLockupComponent,
        SettingCardComponent,
        ScenarioSettingsComponent ,
        AddCardComponent,
        ActiveSessionsComponent,
        SettingFormComponent ,
        GeneralSettingsComponent ,
        GeneralSettingCard ,
        ModulesSettingsComponent,
        CamerasSettingsComponent,
        CameraSettingForm
    ],
    imports : [
        CommonModule ,
        GeneralModule,
        InlineSVGModule,
        FormsModule,
        NgbModule,
        TranslateModule,
        RouterModule.forChild([
        {
            path : '' ,
            component : SettingsComponent
        },
        {
            path : 'addCamera' ,
            component : CameraSettingForm
        },
        {
            path : 'editCamera/:id',
            component : CameraSettingForm
        },
        {
            path : 'add' ,
            component : SettingFormComponent
        },
        {
            path : 'add/module' ,
            component : SettingFormComponent
        },
        {
            path : ':type/:id' ,
            component : SettingFormComponent
        },
        
        
    ])
    ]
})

export class SettingsModule{}