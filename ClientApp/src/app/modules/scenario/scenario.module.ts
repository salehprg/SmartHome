import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { InlineSVGModule } from 'ng-inline-svg';
import { GeneralModule } from 'src/app/partials/general/general.module';
import { HomeModule } from '../home/home.module';
import { ScenarioCardComponent } from './scenario-card/scenario-card.component';
import { ScenariosComponent } from './scenario.component';
import { ScenarioLookupComponent } from './settings-page/scenario-lookup/scenario-lookup.component';
import { ScenarioSettingsPage } from './settings-page/settings-page.component';

@NgModule({
  declarations: [
    ScenariosComponent,
    ScenarioCardComponent,
    ScenarioSettingsPage,
    ScenarioLookupComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    RouterModule,
    InlineSVGModule,
    GeneralModule,
    HomeModule,
  ],
  exports: [ScenariosComponent, ScenarioLookupComponent],
})
export class ScenarioModule {}
