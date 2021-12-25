import { Component } from "@angular/core";
import { TranslateModule } from "@ngx-translate/core";
import { HomeService } from "src/app/modules/home/_services/home.service";

@Component({
    selector : 'sh-scenario-lookup' ,
    templateUrl : './scenario-lookup.component.html' ,
    styleUrls : ['./scenario-lookup.component.scss']
})

export class ScenarioLookupComponent{
    constructor(public homeService:HomeService , public translate:TranslateModule){

    }

    isEmpty(modules: any) {
        let empty = true;
        for (let module in modules) {
          if (modules[module].length > 0) empty = false;
        }
        return empty;
      }
}