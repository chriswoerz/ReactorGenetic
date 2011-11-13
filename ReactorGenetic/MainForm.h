#pragma once
#include "Population.h"


namespace ReactorGenetic {

	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;

	/// <summary>
	/// Summary for MainForm
	///
	/// WARNING: If you change the name of this class, you will need to change the
	///          'Resource File Name' property for the managed resource compiler tool
	///          associated with all .resx files this class depends on.  Otherwise,
	///          the designers will not be able to interact properly with localized
	///          resources associated with this form.
	/// </summary>
	public ref class MainForm : public System::Windows::Forms::Form
	{
	public:
		MainForm(void)
		{
			InitializeComponent();
			//
			//TODO: Add the constructor code here
			//
		}

	protected:
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		~MainForm()
		{
			if (components)
			{
				delete components;
			}
		}
	private: System::Windows::Forms::TextBox^  txtMain;
	protected: 

	private: System::Windows::Forms::Button^  btnStart;
	private: System::Windows::Forms::Button^  btnPause;
	private: System::Windows::Forms::Button^  btnStop;
	private: System::Windows::Forms::Button^  btnClear;
	private: System::Boolean paused;
	private: System::Boolean started;
	private: Population^ population;

	protected: 





	private:
		/// <summary>
		/// Required designer variable.
		/// </summary>
		System::ComponentModel::Container ^components;

#pragma region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		void InitializeComponent(void)
		{
			this->txtMain = (gcnew System::Windows::Forms::TextBox());
			this->btnStart = (gcnew System::Windows::Forms::Button());
			this->btnPause = (gcnew System::Windows::Forms::Button());
			this->btnStop = (gcnew System::Windows::Forms::Button());
			this->btnClear = (gcnew System::Windows::Forms::Button());
			this->paused = false;
			this->SuspendLayout();
			// 
			// txtMain
			// 
			this->txtMain->Location = System::Drawing::Point(13, 13);
			this->txtMain->Multiline = true;
			this->txtMain->Name = L"txtMain";
			this->txtMain->Size = System::Drawing::Size(748, 495);
			this->txtMain->TabIndex = 0;
			// 
			// btnStart
			// 
			this->btnStart->Location = System::Drawing::Point(13, 567);
			this->btnStart->Name = L"btnStart";
			this->btnStart->Size = System::Drawing::Size(75, 23);
			this->btnStart->TabIndex = 1;
			this->btnStart->Text = L"Start";
			this->btnStart->UseVisualStyleBackColor = true;
			this->btnStart->Click += gcnew System::EventHandler(this, &MainForm::btnStart_Click);
			// 
			// btnPause
			// 
			this->btnPause->Location = System::Drawing::Point(95, 567);
			this->btnPause->Name = L"btnPause";
			this->btnPause->Enabled = false;
			this->btnPause->Size = System::Drawing::Size(75, 23);
			this->btnPause->TabIndex = 2;
			this->btnPause->Text = L"Pause";
			this->btnPause->UseVisualStyleBackColor = true;
			this->btnPause->Click += gcnew System::EventHandler(this, &MainForm::btnPause_Click);
			// 
			// btnStop
			// 
			this->btnStop->Location = System::Drawing::Point(176, 567);
			this->btnStop->Name = L"btnStop";
			this->btnStop->Enabled = false;
			this->btnStop->Size = System::Drawing::Size(75, 23);
			this->btnStop->TabIndex = 3;
			this->btnStop->Text = L"Stop";
			this->btnStop->UseVisualStyleBackColor = true;
			this->btnStop->Click += gcnew System::EventHandler(this, &MainForm::btnStop_Click);
			// 
			// btnClear
			// 
			this->btnClear->Location = System::Drawing::Point(257, 567);
			this->btnClear->Name = L"btnClear";
			this->btnClear->Size = System::Drawing::Size(75, 23);
			this->btnClear->TabIndex = 4;
			this->btnClear->Text = L"Clear";
			this->btnClear->UseVisualStyleBackColor = true;
			this->btnClear->Click += gcnew System::EventHandler(this, &MainForm::btnClear_Click);
			// 
			// MainForm
			// 
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(773, 602);
			this->Controls->Add(this->btnClear);
			this->Controls->Add(this->btnStop);
			this->Controls->Add(this->btnPause);
			this->Controls->Add(this->btnStart);
			this->Controls->Add(this->txtMain);
			this->Name = L"MainForm";
			this->Text = L"ReactorGenetic";
			this->ResumeLayout(false);
			this->PerformLayout();

		}
#pragma endregion

	private: System::Void btnStart_Click(System::Object^  sender, System::EventArgs^  e) {
				 Button^ btn = safe_cast<Button^>(sender);
				 btn->Enabled = false;
				 
				 started = true;
				 this->btnPause->Enabled = true;
				 this->btnStop->Enabled = true;

				 if(paused){
					 paused = false;
					 btn->Text = "Start";
					 btnPause->Text = "Pause";
					 btnPause->Enabled = true;
				 }

				 population = gcnew Population();
			     population->Start();
			 }

	private: System::Void btnPause_Click(System::Object^  sender, System::EventArgs^  e) {
 				 paused = true;
				 
				 Button^ btn = safe_cast<Button^>(sender);
			 	 btn->Text = "Paused";
				 btn->Enabled = false;

				 btnStart->Text = "Resume";
				 btnStart->Enabled = true;
				 btnStop->Enabled = true;
			 }
	private: System::Void btnStop_Click(System::Object^  sender, System::EventArgs^  e) {
 				 Button^ btn = safe_cast<Button^>(sender);

				 if(started){
					 started = false;
					 btn->Enabled = false;
					 btnStart->Enabled = true;
				 }
				 if(paused){
					 paused = false;
					 btnStart->Text = "Start";
					 btnPause->Text = "Pause";
				 }				 
			 }
	private: System::Void btnClear_Click(System::Object^  sender, System::EventArgs^  e) {
				 this->txtMain->Clear();
			 }
	};
}

