import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms'
import { Injectable } from '@angular/core'

@Injectable({ providedIn: 'root' })

export class FieldsetCriteriaService {

    constructor(private formBuilder: FormBuilder) { }

    public checkGroupCheckbox(form: FormGroup, allCheckbox: string, arrayName: any, formControlsArray: string): void {
        const selected = form.controls[formControlsArray] as FormArray
        if (selected.length == arrayName.length) {
            document.querySelector<HTMLInputElement>('#' + allCheckbox).checked = true
            form.patchValue({
                [allCheckbox]: true
            })
        }
    }

    public filterList(searchString: string, list: any[]): void {
        list.forEach((element) => {
            const input = document.getElementById(element.description) as HTMLInputElement
            if (input != null || input != undefined) {
                element.description.toLowerCase().includes(searchString.toLowerCase()) == false
                    ? input.classList.add('no-display')
                    : input.classList.remove('no-display')
            }
        })
    }

    public toggleAllCheckboxes(form: FormGroup, array: string, allCheckboxes: string): void {
        const selected = form.controls[array + 's'] as FormArray
        selected.clear()
        const newState = form.value[allCheckboxes]
        const checkboxes = document.querySelectorAll<HTMLInputElement>('.' + array)
        checkboxes.forEach(checkbox => {
            if (newState) {
                checkbox.classList.add('mat-checkbox-checked')
                selected.push(this.formBuilder.group({
                    id: [parseInt(checkbox.id.match(/\d/g).toString()), Validators.required],
                    description: checkbox.outerText
                }))
            } else {
                checkbox.classList.remove('mat-checkbox-checked')
            }
        })
    }

    public updateRadioButtons(form: FormGroup, classname: any, idName: any, id: any, description: any): void {
        const radios = document.getElementsByClassName(classname) as HTMLCollectionOf<HTMLInputElement>
        for (let i = 0; i < radios.length; i++) {
            radios[i].checked = false
        }
        const radio = document.getElementById(idName + id) as HTMLInputElement
        radio.checked = true
        const x = form.controls[classname] as FormArray
        x.clear()
        x.push(new FormControl({
            'id': id,
            'description': description
        }))
    }

    public checkboxChange(form: FormGroup, event: any, allCheckbox: string, formControlsArray: string, array: any[], id: number, description: string): void {
        const selected = form.controls[formControlsArray] as FormArray
        if (event.checked) {
            selected.push(this.formBuilder.group({
                id: [id, Validators.required],
                description: [description]
            }))
        } else {
            const index = selected.controls.findIndex(x => x.value.id == id)
            selected.removeAt(index)
        }
        if (selected.length == 0 || selected.length != array.length) {
            document.querySelector<HTMLInputElement>('#all-' + formControlsArray).checked = false
            form.patchValue({
                [allCheckbox]: false
            })
        } else {
            if (selected.length == array.length) {
                document.querySelector<HTMLInputElement>('#all-' + formControlsArray).checked = true
                form.patchValue({
                    [allCheckbox]: true
                })
            }
        }
    }

}
