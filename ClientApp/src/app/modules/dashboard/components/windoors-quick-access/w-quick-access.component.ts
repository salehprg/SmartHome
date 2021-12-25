import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { TranslateService } from "@ngx-translate/core";
import { TranslationService } from "src/app/modules/i18n/translation.service";

@Component({
    selector : 'sh-windoors-qa' ,
    templateUrl : './w-quick-access.component.html' ,
    styleUrls : ['./w-quick-access.component.scss']
})

export class WindoorsQuickAccessComponent{
    constructor(private router : Router , public translate : TranslateService){}

    goToAll(){
        this.router.navigateByUrl('/windoors/all')
    }
}