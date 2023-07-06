import { Component, OnInit } from '@angular/core';
import { CardsService } from './service/cards.service';
import { Card } from './models/card.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'cards';
  cards:Card[]=[];
  card:Card={
    id:'',
    cardholderName:'',
    cardNumber:'',
    expireMonth:'',
    expireYear:'',
    cvc:''
  }
  constructor(private cardService:CardsService){

  }
  ngOnInit(): void {
      this.getAllCards();
  }
  getAllCards(){
    this.cardService.getAllCards()
    .subscribe(
      response=>{
        this.cards=response;
        console.log(response)
      }
    )
    
  }
  onSubmit(){

    if(this.card.id==''){
      this.cardService.addCard(this.card)
      .subscribe(
        response=>{
          this.resetForm();
          
          this.getAllCards();
          console.log(response);
        }
      )
    }else{
     this.updateCard(this.card)
    }
   
    
  }

  deleteCard(id:string){
    this.cardService.deleteCard(id)
    .subscribe(
      response=>{
        this.getAllCards();
      }
    );
  }
  populateForm(card:Card){
    this.card=card
    console.log(card)
  }
  updateCard(card:Card){
    this.cardService.updateCard(card)
    .subscribe(
      response=>{
        this.resetForm()
        this.getAllCards();
      }
    )
  }  
  resetForm() {
    this.card = {
      id: '',
      cardholderName: '',
      cardNumber: '',
      expireMonth: '',
      expireYear: '',
      cvc: ''
    };
  }
}
